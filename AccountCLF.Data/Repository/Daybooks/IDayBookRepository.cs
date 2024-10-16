using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repository.Daybooks
{
    public interface IDayBookRepository
    {
        Task<List<Daybook>> GetAll();
        Task<List<Daybook>> GetAccountBalance();
        Task<decimal> GetAccountBalanceByEntityIdAsync(int entityID);
        Task<Daybook> GetDaybookById(int id);
    }
}
