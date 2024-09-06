using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Entities
{
    public class GetEntityBankandAccountNumberDto
    {
        public int EntityId { get; set; }
        public string Details { get; set; }
    }
}
