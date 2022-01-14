using Messages;

namespace CommandService.EventProcessing;

public interface IEventProcessor
{
    void ProccesEvent(Message message);
}