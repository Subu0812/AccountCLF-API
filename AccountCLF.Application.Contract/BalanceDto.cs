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
        public DateTime? Date { get; set; }
        public string? Amount { get; set; }
        public string Balance { get; set; }
        public Tds TdsPaid { get; set; }
        public List<Deduction> Deductions { get; set; } = new List<Deduction>();

    }
    public class Tds
    {
        public int? SectionId { get; set; }
        public decimal? TdsableAmount { get; set; }

    }
    public class Deduction
    {
        public string? TransType { get; set; }
        public string? Amount { get; set; }
    }
}
