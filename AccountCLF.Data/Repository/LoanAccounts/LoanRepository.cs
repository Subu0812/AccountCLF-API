using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.LoanAccounts
{
    public class LoanRepository : ILoanAccountRepository
    {
        private readonly AccountClfContext _context;
        public LoanRepository(AccountClfContext context)
        {
            _context = context;
        }

        public async Task<List<LoanAccount>> GetAsyncWithAll()
        {
            return await _context.LoanAccounts
                           .Include(x => x.LoanAccountDetails)
                           .Include(x=>x.Loantenure)
                           .Include(x => x.Entity).ThenInclude(x => x.BasicProfiles)
                           .ToListAsync();
        }

        public async Task<LoanAccount> GetByEntityId(int entityId)
        {
            return await _context.LoanAccounts
          .Include(x => x.LoanAccountDetails)
          .FirstOrDefaultAsync(x => x.EntityId == entityId);
        }
        public async Task<List<LoanAccount>> GetByEntityIdAsync(int entityId)
        {
            return await _context.LoanAccounts
                .Include(x => x.LoanAccountDetails)
                .Include(x => x.Loantenure)
                .Where(detail => detail.EntityId == entityId )
                .ToListAsync();
        }

        public async Task<List<LoanAccountDetail>> GetLoanDetailByEntityIdAsync(int entityId)
        {
            return await _context.LoanAccountDetails
                          .Where(detail => detail.EntityId == entityId)
                          .ToListAsync();
        }

        public async Task<List<LoanAccountDetail>> GetLoanDetailByLoanAccountId(int loanAccountId)
        {
            return await _context.LoanAccountDetails
                .Where(x=>x.LoanAccountId == loanAccountId)
                .ToListAsync();
         }

     
    }
}
