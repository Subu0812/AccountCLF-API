using AccountCLF.Application.Contract.Entities.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Entities
{
    public class GetEntityFilterCLFPOSHGDto
    {
        public int ID { get; set; }
        public List<GetBasicProfileDto> BasicProfiles { get; set; } = new List<GetBasicProfileDto>();

    }
}
