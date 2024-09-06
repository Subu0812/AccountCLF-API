using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repository.LoanAccounts
{
    public interface ILoanAccountRepository
    {

        Task<LoanAccount> GetByEntityId(int entityId);
        Task<List<LoanAccountDetail>> GetLoanDetailByLoanAccountId(int loanAccountId);
        Task<List<LoanAccount>> GetByEntityIdAsync(int entityId);
        Task<List<LoanAccountDetail>> GetLoanDetailByEntityIdAsync(int entityId);
        Task<List<LoanAccount>> GetAsyncWithAll();


    }
}
