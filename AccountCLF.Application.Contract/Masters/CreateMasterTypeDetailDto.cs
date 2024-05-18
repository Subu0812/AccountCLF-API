namespace AccountCLF.Application.Contract.Masters;

public class CreateMasterTypeDetailDto
{
    public decimal? SrNo { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? ParentId { get; set; }

    public int? TypeId { get; set; }

    public int? IsActive { get; set; }

    public int? IsDelete { get; set; }

    public DateTime? Date { get; set; }
}
