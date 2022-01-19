using MessageModels;

namespace CommandService.EventProcessing;

public interface IEventProcessor
{
    void ProccesEvent(PlatformMessage message);
}