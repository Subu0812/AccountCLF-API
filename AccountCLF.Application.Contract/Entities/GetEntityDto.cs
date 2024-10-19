using AccountCLF.Application.Contract.Entities.Logins;
using AccountCLF.Application.Contract.Masters;
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
        public  GetMasterTypeDetailsDto? Type { get; set; }
        public virtual GetAccountGroupDto? AccountType { get; set; }

        public virtual GetParentEntityDto? Parent { get; set; }

        public List<GetBasicProfileDto> BasicProfiles { get; set; } = new List<GetBasicProfileDto>();
        public virtual GetReferenceEntityDto? Reference { get; set; }
        public virtual ICollection<GetAddressDto> AddressDetails { get; set; } = new List<GetAddressDto>();
        public virtual ICollection<GetBankDetailDto> BankDetails { get; set; } = new List<GetBankDetailDto>();
        public virtual ICollection<GetDocumentDto> DocumentProfiles { get; set; } = new List<GetDocumentDto>();
        public virtual ICollection<GetContactProfileDto> ContactProfiles { get; set; } = new List<GetContactProfileDto>();
        public virtual ICollection<GetMasterLoginDto> MasterLogins { get; set; } = new List<GetMasterLoginDto>();
        public virtual ICollection<GetProfileLinkDto> ProfileLinks { get; set; } = new List<GetProfileLinkDto>();


    }
    public class GetProfileLinkDto
    {
        public int Id { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
    }
    public class GetMasterLoginDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
    public class GetContactProfileDto
    {
        public int Id { get; set; } 
        public int? ContactTypeId { get; set; }
        public string? MobileNo { get; set; }
        public string Email { get; set; }
    }
    public class GetDocumentDto
    {
        public int Id { get; set; }
        public decimal? SrNo { get; set; }
        public int? DocType { get; set; }
        public string? Path { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class GetAddressDto
    {
        public int Id { get; set; }
        public int? AddressTypeId { get; set; }
        public int? CityId { get; set; }
        public string? PinCode { get; set; }
        public string? Address { get; set; }
        public string? LandMark { get; set; }
    }
    public class GetBankDetailDto
    {
        public int Id { get; set; }
        public int? SrNo { get; set; }
        public string BeneficiaryName { get; set; } = null!;
        public string AccountNo { get; set; } = null!;
        public string Ifsccode { get; set; } = null!;
        public int? ParentId { get; set; }
        public int BankId { get; set; }
    }
    public class GetParentEntityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class GetReferenceEntityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    
}
