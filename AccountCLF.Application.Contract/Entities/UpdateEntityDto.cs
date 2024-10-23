using Microsoft.AspNetCore.Http;

namespace AccountCLF.Application.Contract.Entities
{
    public class UpdateEntityDto
    {
        public int Id { get; set; }

        public int TypeId { get; set; }
        public DateTime? Date { get; set; }
        public int SessionId { get; set; }
        public int? ReferenceId { get; set; }
        public int? StaffId { get; set; }
        public string Name { get; set; }
   
        public GetContactProfileDto ContactProfiles { get; set; } 
        public GetMasterLoginDto MasterLogins { get; set; } 
        public GetProfileLinkDto ProfileLinks { get; set; }
 
    }
 
    public class UpdateDocumentDto
    {
        public decimal? DocumentSrNo { get; set; }
        public int? DocType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImagePath { get; set; }
    }
    public class UpdateAddressDto
    {
        public int? AddressTypeId { get; set; }
        public int? CityId { get; set; }
        public string? PinCode { get; set; }
        public string? Address { get; set; }
        public string? LandMark { get; set; }
    }
    public class UpdateBankDetailDto
    
        {
        public int? SrNo { get; set; }
        public string BeneficiaryName { get; set; } = null!;
        public string AccountNo { get; set; } = null!;
        public string Ifsccode { get; set; } = null!;
        public int? ParentId { get; set; }
        public int BankId { get; set; }
    }


}

