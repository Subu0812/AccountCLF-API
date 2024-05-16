using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Masters.MasterType
{
    public class CreateMasterTypeDto
    {
        public decimal? SrNo { get; set; }

        public string? Name { get; set; }

        public int? ParentId { get; set; }

        public DateTime? Date { get; set; }

        public int? IsDelete { get; set; }

        public int? IsActive { get; set; }
    }
}
