using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.MasterTypeDetails
{
    public class MasterTypeRepository : IMasterTypeRepository
    {
        private readonly DataContext _dataContext;
        public MasterTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<MasterTypeDetail>> GetByTypeName(string name)
        {
            var data = await _dataContext.MasterTypeDetails
                .Include(x => x.Type)
                .Where(x => x.Type.Name == name)
                .ToListAsync();
            return data;
        }

        public async Task<bool> UpdateIsActive(int id, int isActive)
        {
            var data = await _dataContext.Locations.FindAsync(id);
            if (data == null)
            {
                return false;
            }
            data.IsActive = isActive;
            _dataContext.Locations.Update(data);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
