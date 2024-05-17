using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Locations
{
    public class CreateLocationDto
    {
        public string? SrNo { get; set; }

        public string? Code { get; set; }

        public string? ShortName { get; set; }

        public string? Name { get; set; }

        public int? ParentId { get; set; }

        public int? TypeId { get; set; }

        public int? IsActive { get; set; }
    }
}
