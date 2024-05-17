using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Entities
{
    public class CreateEntityDto
    {
        public int? TypeId { get; set; }

        public DateTime? Date { get; set; }

        public int? AccountTypeId { get; set; }

        public int? SessionId { get; set; }

        public int? ReferenceId { get; set; }

        public int? StaffId { get; set; }

        public int? Status { get; set; }

        public int? IsActive { get; set; }



        public int? EntityId { get; set; }

        public string? FatherName { get; set; }

        public string? MotherName { get; set; }





        public string? Password { get; set; }



        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Designation { get; set; }



        public int? ContactTypeId { get; set; }

        public string? MobileNo { get; set; }

        public string? Email { get; set; }
    }
}
