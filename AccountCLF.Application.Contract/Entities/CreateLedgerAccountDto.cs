using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Entities
{
    public class CreateLedgerAccountDto
    {
        public int? AccountTypeId { get; set; }
        public string? Name { get; set; }
        public int? SessionId { get; set; }

    }
}
