using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Models;

namespace PlatformService.Data;
public static class PrepDb
{
    public static async void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), serviceScope.ServiceProvider.GetService<IMessageBusPublisher>());
        }
    }

    /// <summary>
    /// Seeds initial data to DB and Publishes them to MessageBus
    /// </summary>
    /// <param name="context"></param>
    /// <param name="publisher"></param>
    /// <returns></returns>
    private static async Task SeedData(AppDbContext context, IMessageBusPublisher publisher)
    {
        if (await context.Platforms.AnyAsync())
        {
            Console.WriteLine("--> Already have data.");
            return;
        }

        Console.WriteLine("--> Seeding data...");
        Platform[] platforms = {
                new Platform() { Name = ".Net", Publisher = "Microsoft" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation" }
                };

        await context.Platforms.AddRangeAsync(platforms);
        await context.SaveChangesAsync();

        foreach (var platform in platforms)
        {
            await publisher.PublishPlatform(platform);
        }
    }

}
