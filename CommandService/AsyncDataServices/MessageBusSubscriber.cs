using CommandService.EventProcessing;
using EasyNetQ;
using MessageModels;

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
        // await _bus.PubSub.SubscribeAsync<PlatformMessage>("platform", _eventProcessor.ProccesEvent);
        // await _bus.SendReceive.ReceiveAsync<PlatformMessage>("Platform.Data", message =>  _eventProcessor.ProccesEvent(message));
        await _bus.SendReceive.ReceiveAsync("Platform.Data", x => x.Add<PlatformMessage>(message => _eventProcessor.ProccesEvent(message)));
        Console.WriteLine("--> Listening for Messages");
 
    }

}