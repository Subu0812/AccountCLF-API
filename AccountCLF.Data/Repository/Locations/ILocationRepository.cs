using Model;

namespace AccountCLF.Data.Repository.Locations
{
    public interface ILocationRepository
    {
        Task<Location> UpdateIsActive(int id);
        Task<List<Location>> Get();
        Task<Location> GetById(int id);
    }
}
