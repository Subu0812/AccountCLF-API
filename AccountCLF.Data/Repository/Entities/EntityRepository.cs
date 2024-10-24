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
                .Include(x=>x.DocumentProfiles.Where(x => x.IsDelete == false))
                .Include(x=>x.MasterLogins)
                .Include(x=>x.ProfileLinks)
                .Include(x => x.AddressDetails.Where(x => x.IsDelete == false))
                .Include(x => x.Type)
                .Include(x => x.AccountType)
                .Include(x => x.Reference)
                .Include(x => x.BankDetails.Where(x => x.IsDelete == false))
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
                 .Include(x => x.BasicProfiles)
                .Include(x => x.ContactProfiles)
                .Include(x => x.DocumentProfiles.Where(x => x.IsDelete == false))
                .Include(x => x.MasterLogins)
                .Include(x => x.ProfileLinks)
                .Include(x=>x.AddressDetails.Where(x => x.IsDelete == false))
                .ThenInclude(x=>x.City)
                .ThenInclude(X=>X.Parent)
                .ThenInclude(X=>X.Parent)
                .Include(x => x.Type)
                .Include(x => x.AccountType)
                .Include(x => x.Reference)
                .Include(x => x.BankDetails.Where(x=>x.IsDelete==false))
                .ThenInclude(x => x.Bank)
                .Where(x => x.Id == Id && x.IsDelete==false )
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
