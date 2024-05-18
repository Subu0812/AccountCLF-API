using Model;

namespace AccountCLF.Data.Repository.MasterTypeDetails
{
    public interface IMasterTypeRepository
    {
        Task<bool> UpdateIsActive(int id, int isActive);
        Task<List<MasterTypeDetail>> GetByTypeName(string name);

    }
}
