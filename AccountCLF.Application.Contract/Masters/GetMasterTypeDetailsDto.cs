using AccountCLF.Application.Contract.Masters.MasterType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Masters
{
    public class GetMasterTypeDetailsDto
    {
        public int Id { get; set; }

        public decimal? SrNo { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? ParentId { get; set; }

        public int? TypeId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }
        public string? Value { get; set; }


        public DateTime? Date { get; set; }
        public  GetMasterTypeDto? Type { get; set; }
    }
}
