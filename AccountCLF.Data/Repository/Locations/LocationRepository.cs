using Data;

namespace AccountCLF.Data.Repository.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext _dataContext;
        public LocationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> UpdateIsActive(int id, int isActive)
        {
            var data = await _dataContext.Locations.FindAsync(id);
            if (data != null)
            {
                data.IsActive = isActive;
                _dataContext.Locations.Update(data);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
