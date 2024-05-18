using Model;

namespace AccountCLF.Data.Repositories.Masters.MasterType;

public interface IMasterTypeRepository
{
    Task<List<MasterTypeDetail>> Get(string name);
    Task<MasterTypeDetail> UpdateStatus(int id, int isActive);
}
