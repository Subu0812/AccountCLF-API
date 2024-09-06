using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repository.Entities
{
    public interface IEntityRepository
    {
        Task<MasterLogin> GetUserByEmail(string username);
        Task<BasicProfile> GetBasicProfileByEntityId(int entityId); 
        Task<Entity> GetById(int Id); 
        Task<List<Entity>> GetAll(); 


    }
}
