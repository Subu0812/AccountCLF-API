using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Charges_Expenses
{
    public class GetAccountBalanceReportDto
    {
        public int Id { get; set; }
        public string? HolderName { get; set; }
        public string? ParticularName { get; set; }
        public string? AccountBalance { get; set; }
        public decimal? Amount { get; set; }
        public string? TransType { get; set; }
        public int? PayModeId { get; set; }
        public DateTime? Date { get; set; }
    }
}
