using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.TransFunds.Bills
{
    public class CreateFundBillDto
    {
        public int FundReferenceId { get; set; }

        public string BillNo { get; set; }


    }
}
