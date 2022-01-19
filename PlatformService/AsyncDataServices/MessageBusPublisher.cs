using AutoMapper;
using EasyNetQ;
using MessageModels;
using PlatformService.Models;

namespace PlatformService.AsyncDataServices;

public class MessageBusPublisher : IMessageBusPublisher
{
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public MessageBusPublisher(IBus bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }

    /// <summary>
    /// Publishes messages to MessageBus
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public async Task PublishPlatform(Platform platform)
    {
        var message = _mapper.Map<PlatformMessage>(platform);
        message.Event = "Platform_Published";
        // await _bus.PubSub.PublishAsync<PlatformMessage>(message);
        await _bus.SendReceive.SendAsync("Platform.Data", message);
        Console.WriteLine($"Message published!");
    }
}