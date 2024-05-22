using Model;

namespace AccountCLF.Data.Repository.Locations
{
    public interface ILocationRepository
    {
        Task<bool> UpdateIsActive(int id);
        Task<List<Location>> Get();
        Task<Location> GetById(int id);
    }
}
