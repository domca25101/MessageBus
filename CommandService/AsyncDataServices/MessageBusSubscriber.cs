using CommandService.EventProcessing;
using EasyNetQ;
using Messages;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IBus _bus;
    private readonly IEventProcessor _eventProcessor;

    public MessageBusSubscriber(IBus bus, IEventProcessor eventProcessor)
    {
        _bus = bus;
        _eventProcessor = eventProcessor;
    }
    
    /// <summary>
    /// Message Listener
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        await _bus.PubSub.SubscribeAsync<Message>("platform", _eventProcessor.ProccesEvent);
        Console.WriteLine("--> Listening for Messages");

    }

}