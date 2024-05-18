using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repositories.Masters.MasterType
{
    public interface IMasterTypeRepository 
    {
        Task<List<MasterTypeDetail>> Get(string name);
        Task<MasterTypeDetail> UpdateStatus(int id, int isActive);
    }
}
