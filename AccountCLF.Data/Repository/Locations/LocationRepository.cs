using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext _dataContext;
        public LocationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Location>> Get()
        {
            return await _dataContext.Locations
                .Include(x=>x.Type)
                .Include(x=>x.Parent)
                .ThenInclude(x=>x.Type)
                .ToListAsync();
        }

        public async Task<Location> GetById(int id)
        {
            return await _dataContext.Locations
                .Include(x => x.Type)
                .Include(x => x.Parent)
                .ThenInclude(x => x.Type)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<bool> UpdateIsActive(int id)
        {
            var data = await _dataContext.Locations.FindAsync(id);
            if (data != null)
            {
                data.IsActive = !data.IsActive;
                _dataContext.Locations.Update(data);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
