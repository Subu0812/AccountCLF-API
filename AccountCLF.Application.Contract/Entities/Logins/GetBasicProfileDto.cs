namespace AccountCLF.Application.Contract.Entities.Logins
{
    public class GetBasicProfileDto
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

    }
}
