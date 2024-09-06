using Model;

namespace AccountCLF.Data.Repository.Locations
{
    public interface ILocationRepository
    {
        Task<Location> UpdateIsActive(int id);
        Task<List<Location>> Get();
        Task<List<Location>> GetState();
        Task<Location> GetById(int id);
        Task<List<Location>> GetBlockByCityId(int cityId);
        Task<List<Location>> GetCityByStateId(int stateId);


    }
}
