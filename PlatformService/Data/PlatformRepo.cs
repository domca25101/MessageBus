using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Creates Platform
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
    /// Returns list of all existing platforms
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms.ToList();
    }

    /// <summary>
    /// Returns platform with specified Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Platform GetPlatformById(int id)
    {
        return _context.Platforms.FirstOrDefault(p => p.Id == id);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }
}