using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.Entities
{
    public class EntityRepository : IEntityRepository
    {
        private readonly AccountClfContext _context;
        public EntityRepository(AccountClfContext context)
        {
            _context = context;
        }

        public async Task<List<Entity>> GetAll()
        {
            return await _context.Entities
                .Include(x => x.BasicProfiles)
                .Include(x => x.ContactProfiles)
                .Include(x => x.Type)
                .Include(x => x.AccountType)
                .Include(x => x.Reference)
                .Include(x => x.BankDetails)
                .ThenInclude(x => x.Bank)
                .Where(x=>x.IsDelete==false)
                .ToListAsync();
        }

        public async Task<BasicProfile> GetBasicProfileByEntityId(int entityId)
        {
            return await _context.BasicProfiles.Where(x => x.EntityId == entityId).FirstOrDefaultAsync();
        }

        public async Task<Entity> GetById(int Id)
        {
            return await _context.Entities
                .Include(x=>x.BasicProfiles)
                .Include(x => x.Type)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<MasterLogin> GetUserByEmail(string username)
        {
            var data = await _context.MasterLogins
                .Include(x => x.Entity)
                .ThenInclude(x => x.BasicProfiles)
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (data == null)
            {
                return null;
            }
            return data;
        }
    }
}
