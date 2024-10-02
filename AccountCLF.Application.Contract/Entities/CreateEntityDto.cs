using System.Collections.ObjectModel;

namespace AccountCLF.Application.Contract.Entities
{
    public class CreateEntityDto
    {
        public int TypeId { get; set; }
        public DateTime? Date { get; set; }
        //public int? AccountTypeId { get; set; }
        public int SessionId { get; set; }
        public int? ReferenceId { get; set; }
        public int? StaffId { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? Password { get; set; }
        public string Name { get; set; }
        public int? ContactTypeId { get; set; }
        public string? MobileNo { get; set; }
        public string Email { get; set; }
        public List<CreateAddressDto>? SampleAddresses { get; set; }= new List<CreateAddressDto>(){ };
        public List<CreateBankDetailDto>? SampleBankDetails { get; set; } = new List<CreateBankDetailDto>();
        public List<CreateDocumentDto>? SampleDocuments { get; set; } = new List<CreateDocumentDto>();
    }
    public class CreateDocumentDto
    {
        public decimal? DocumentSrNo { get; set; }
        public int? DocType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }
    }
    public class CreateAddressDto
    {
        public int? AddressTypeId { get; set; }
        public int? CityId { get; set; }
        public string? PinCode { get; set; }
        public string? Address { get; set; }
        public string? LandMark { get; set; }
    }
    public class CreateBankDetailDto
    {
        public int? SrNo { get; set; }
        public string BeneficiaryName { get; set; } = null!;
        public string AccountNo { get; set; } = null!;
        public string Ifsccode { get; set; } = null!;
        public int? ParentId { get; set; }
        public int BankId { get; set; }
    }
}
