using Model;

namespace AccountCLF.Data.Repository.MasterTypeDetails
{
    public interface IMasterTypeRepository
    {
        Task<bool> UpdateIsActive(int id);
        Task<bool> UpdateDetailsIsActive(int id);
        Task<List<MasterTypeDetail>> GetByTypeName(string name);
        Task<List<MasterTypeDetail>> Get();
        Task<MasterTypeDetail> GetById(int id);

    }
}
