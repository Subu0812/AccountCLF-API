using AccountCLF.Application.Contract.TransFunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract
{
    public class BalanceDto
    {
        public int Id { get; set; }
        public int? FranchiseId { get; set; }
        public string? TransType { get; set; }
        //public DateTime Date { get; set; }

        public DateTime? Date { get; set; }
        public string? Amount { get; set; }
        public string Balance { get; set; }

    }
}
