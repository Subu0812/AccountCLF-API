using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AccountClfContext _dataContext;
        public LocationRepository(AccountClfContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Location>> Get()
        {
            return await _dataContext.Locations
                .Include(x=>x.Type)
                .Include(x=>x.Parent)
                .ToListAsync();
        }

        public async Task<List<Location>> GetBlockByCityId(int cityId)
        {
            return await _dataContext.Locations
                            .Where(x => x.ParentId == (int)cityId)
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

        public async Task<List<Location>> GetCityByStateId(int stateId)
        {
            return await _dataContext.Locations
                .Where(x=>x.ParentId==(int)stateId)
                .ToListAsync();
        }

        public async Task<List<Location>> GetState()
        {
         return await _dataContext.Locations
                .Include(x=>x.Type).Include(x=>x.Parent)
                .Where(x=>x.Type.Name.ToLower()=="state")
                .ToListAsync();
        }

        public async Task<Location> UpdateIsActive(int id)
        {
            var data = await _dataContext.Locations.FindAsync(id);
            if (data != null)
            {
                data.IsActive = !data.IsActive;
                _dataContext.Locations.Update(data);
                await _dataContext.SaveChangesAsync();
                return data;
            }
            return null;
        }
    }
}
