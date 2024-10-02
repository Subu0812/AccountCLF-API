namespace AccountCLF.Application.Contract.Entities
{
    public class UpdateEntityDto
    {
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public int? AccountTypeId { get; set; }
        public int? TypeId { get; set; }


    }
}
