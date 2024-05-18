using Data;
using Model;

namespace AccountCLF.Data.Repositories.Locations;

public class LocationRepository : ILocationRepository
{
    private readonly DataContext _context;
    public LocationRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<Location> UpdateStatus(int id, int isActive)
    {
        var data = await _context.Locations.FindAsync(id);
        if (data != null)
        {
            data.IsActive = isActive;
            _context.Locations.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }
        return null;
    }
}
