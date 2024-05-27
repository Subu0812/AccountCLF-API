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
        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }

    }
}
