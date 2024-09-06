using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.TDSs
{
    public class GetTransFundTdsDto
    {
        public int Id { get; set; }

        public int? FundReferenceId { get; set; }

        public decimal? TdsableAmount { get; set; }

        public decimal? Tdsper { get; set; }

        public decimal? Tds { get; set; }

        public string? PanNo { get; set; }
    }
}
