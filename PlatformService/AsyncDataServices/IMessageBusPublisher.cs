using PlatformService.Models;

namespace PlatformService.AsyncDataServices;

public interface IMessageBusPublisher
{
    Task PublishPlatform(Platform platform);
}