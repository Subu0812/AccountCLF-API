using AccountCLF.Application.Contract.Masters;
using AccountCLF.Application.Contract.Masters.MasterType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Locations
{
    public class GetLocationDto
    {
        public int Id { get; set; }

        public string? SrNo { get; set; }

        public string? Code { get; set; }

        public string? ShortName { get; set; }

        public string? Name { get; set; }

        public int? ParentId { get; set; }

        public int? TypeId { get; set; }

        public bool? IsActive { get; set; }

        public GetParentLocationDto? Parent { get; set; }

        public  GetMasterTypeDetailsDto? Type { get; set; }

        public class GetParentLocationDto
        {
            public int Id { get; set; }

            public string? SrNo { get; set; }

            public string? Code { get; set; }

            public string? ShortName { get; set; }

            public string? Name { get; set; }

            public int? ParentId { get; set; }

            public bool? IsActive { get; set; }

        }
    }
}
