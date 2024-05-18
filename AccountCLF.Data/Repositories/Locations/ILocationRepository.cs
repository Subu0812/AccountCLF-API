using Model;

namespace AccountCLF.Data.Repositories.Locations;

public interface ILocationRepository
{
    Task<Location> UpdateStatus(int id, int isActive);
}
