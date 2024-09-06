using Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace AccountCLF.Data.Repository.Daybooks
{
    public class DayBookRepository : IDayBookRepository
    {
        private readonly AccountClfContext _dataContext;
        public DayBookRepository(AccountClfContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Daybook>> GetAccountBalance()
        {
            return await _dataContext.Daybooks
                .Include(x => x.FundReference) 
                .ThenInclude(x=>x.TransFundTds)
                .Include(x => x.Account)
                .Include(x => x.Franchise) 
                    .ThenInclude(f => f.BasicProfiles) 
                    .Include(f => f.Franchise. BankDetails) 
                .Include(x => x.Parent) 
                
                .ToListAsync();
        }
        public async Task<decimal> GetAccountBalanceByEntityIdAsync(int entityId)
        {
            // Fetch all daybook entries
            var daybookEntries = await _dataContext.Daybooks.ToListAsync();            
            var relevantEntries = daybookEntries.Where(d => d.FranchiseId == entityId).ToList();

            // Calculate account balance
            decimal accountBalance = 0;
            foreach (var entry in relevantEntries)
            {
                if (entry.TransType == "DR")
                {
                    accountBalance += entry.Amount ?? 0;
                }
                else if (entry.TransType == "CR")
                {
                    accountBalance -= entry.Amount ?? 0;
                }
            }

            return accountBalance;
        }

        public async Task<List<Daybook>> GetAll()
        {
            return await _dataContext.Daybooks
                .Include(x => x.FundReference)
                .ThenInclude(x => x.LedgerHead)
                .ToListAsync();
        }
    }
}
