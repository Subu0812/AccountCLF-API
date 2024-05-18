namespace AccountCLF.Data.Repository.Locations
{
    public interface ILocationRepository
    {
        Task<bool> UpdateIsActive(int id, int isActive);
    }
}
