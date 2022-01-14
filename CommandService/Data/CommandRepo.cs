using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates Command for platform with specified Id
    /// </summary>
    /// <param name="platformId"></param>
    /// <param name="command"></param>
    public void CreateCommand(int platformId, Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }

    /// <summary>
    /// Determine whether External platform with specified Id exist
    /// </summary>
    /// <param name="externalPlatformId"></param>
    /// <returns></returns>
    public bool ExternalPlatformExist(int externalPlatformId)
    {
        return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
    }

    /// <summary>
    /// Creates Platform for Db context based on given platform
    /// </summary>
    /// <param name="platform"></param>
    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }
        _context.Platforms.Add(platform);
    }

    /// <summary>
    /// Returns list of all platforms with their commands
    /// </summary>
    /// <returns>list</returns>
    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms
        .Include(p => p.Commands).ToList();
    }

    /// <summary>
    /// Returns command with specified Id for specified platform
    /// </summary>
    /// <param name="platformId"></param>
    /// <param name="commandId"></param>
    /// <returns></returns>
    public Command GetCommand(int platformId, int commandId)
    {
        return _context.Commands
        .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
    }

    /// <summary>
    /// returns all commands for specified platform
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
    }

    /// <summary>
    /// Determines whether platform with specified Id exist
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    public bool PlatformExist(int platformId)
    {
        return _context.Platforms.Any(p => p.Id == platformId);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }
}
