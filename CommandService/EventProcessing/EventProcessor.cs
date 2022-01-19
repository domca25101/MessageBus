using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using MessageModels;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Processes message based on Event written inside message
    /// </summary>
    /// <param name="message"></param>
    public void ProccesEvent(PlatformMessage message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Determines Type of the event
    /// </summary>
    /// <param name="platformPublishedDto"></param>
    /// <returns></returns>
    private EventType DetermineEvent(PlatformMessage message)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = _mapper.Map<GenericEventDto>(message);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Determined");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("-->Could Not Determine Event");
                return EventType.Undetermined;
        }
    }

    /// <summary>
    /// Adds platform to DB
    /// </summary>
    /// <param name="message"></param>
    private void AddPlatform(PlatformMessage message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            try
            {
                var platform = _mapper.Map<Platform>(message);
                if (!repository.ExternalPlatformExist(platform.ExternalId))
                {
                    repository.CreatePlatform(platform);
                    repository.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> Platform already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not add to DB {ex.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
