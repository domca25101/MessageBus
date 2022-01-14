using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Data;
using PlatformService.Models;
using EasyNetQ;
using Messages;
using PlatformService.AsyncDataServices;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly IMessageBusPublisher _publisher;

    public PlatformController(IPlatformRepo repository, IMapper mapper, IMessageBusPublisher publisher)
    {
        _repository = repository;
        _mapper = mapper;
        _publisher = publisher;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> getting platforms...");
        var platformItems = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine("--> Getting platform");
        var platformItem = _repository.GetPlatformById(id);
        if (platformItem != null)
        {
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        _repository.CreatePlatform(platform);
        _repository.SaveChanges();

        try
        {
            // await _commandDataClient.SendPlatformToCommand(platformReadDto);
            await _publisher.PublishPlatform(platform);
            // var message= _mapper.Map<Message>(platformReadDto);
            // message.Event ="Platform_Published";
            // await _bus.PubSub.PublishAsync<Message>(message);
            // Console.WriteLine("Message published!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        
        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }
}