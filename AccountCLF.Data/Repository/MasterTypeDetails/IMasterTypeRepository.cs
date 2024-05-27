using Model;

namespace AccountCLF.Data.Repository.MasterTypeDetails
{
    public interface IMasterTypeRepository
    {
        Task<MasterType> UpdateIsActive(int id);
        Task<MasterTypeDetail> UpdateDetailsIsActive(int id);
        Task<List<MasterTypeDetail>> GetByTypeName(string name);
        Task<List<MasterTypeDetail>> Get();
        Task<MasterTypeDetail> GetById(int id);

    }
}
