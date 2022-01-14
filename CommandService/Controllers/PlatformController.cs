using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly ICommandRepo _repository;

    public PlatformController(ICommandRepo repository, IMapper mapper, AppDbContext context)
    {
       _repository = repository;
        _mapper = mapper;
        _context = context;
        
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
    {
        var platformItems = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
}
