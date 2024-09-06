using AccountCLF.Application.Contract.Entities.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Entities
{
    public class GetEntityDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? TypeId { get; set; }

        public DateTime? Date { get; set; }

        public int? AccountTypeId { get; set; }

        public int? SessionId { get; set; }

        public int? ReferenceId { get; set; }

        public int? StaffId { get; set; }

        public int? Status { get; set; }

        public int? IsActive { get; set; }
        public List<GetBasicProfileDto> BasicProfiles { get; set; } = new List<GetBasicProfileDto>();

    }
}
