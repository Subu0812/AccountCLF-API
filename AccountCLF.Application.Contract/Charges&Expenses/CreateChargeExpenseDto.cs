using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract;

public class CreateChargeExpenseDto
{
    public int? AccountId { get; set; }
    public int FranchiseId { get; set; }
    public int? FundReferenceId { get; set; }
    public int? SessionId { get; set; }
    public decimal? Amount { get; set; }
public string Description { get; set; }

}
