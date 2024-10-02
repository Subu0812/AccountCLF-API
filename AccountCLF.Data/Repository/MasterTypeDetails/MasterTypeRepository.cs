using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.MasterTypeDetails
{
    public class MasterTypeRepository : IMasterTypeRepository
    {
        private readonly AccountClfContext _dataContext;
        public MasterTypeRepository(AccountClfContext dataContext)
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

        public async Task<MasterType> UpdateIsActive(int id)
        {
            var data = await _dataContext.MasterTypes.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            data.IsActive = !data.IsActive;
            _dataContext.MasterTypes.Update(data);
            await _dataContext.SaveChangesAsync();
            return data;
        }
        public async Task<MasterTypeDetail> UpdateDetailsIsActive(int id)
        {
            var data = await _dataContext.MasterTypeDetails.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            data.IsActive = !data.IsActive;
            _dataContext.MasterTypeDetails.Update(data);
            await _dataContext.SaveChangesAsync();
            return data;
        }

        public async Task<List<MasterTypeDetail>> Get()
        {
            return await _dataContext.MasterTypeDetails.Include(x => x.Type).ToListAsync();
        }

        public async Task<MasterTypeDetail> GetById(int id)
        {
            return await _dataContext.MasterTypeDetails
                .Include(x =>x.Parent)
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
