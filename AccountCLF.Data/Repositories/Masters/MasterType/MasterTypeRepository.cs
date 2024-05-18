using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repositories.Masters.MasterType
{
    public class MasterTypeRepository : IMasterTypeRepository
    {
        private readonly DataContext _context;
        public MasterTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<MasterTypeDetail>> Get(string name)
        {
            var data = await _context.MasterTypeDetails
                .Include(x => x.Type)
                .Where(x=>x.Type.Name.ToLower()== name.ToLower())
                .ToListAsync();
            return data;
        }

        public async Task<MasterTypeDetail> UpdateStatus(int id, int isActive)
        {
            var data = await _context.MasterTypeDetails.FindAsync(id);
            if (data != null)
            {
                data.IsActive = isActive;
              var result=  _context.MasterTypeDetails.Update(data);
              await  _context.SaveChangesAsync();
                return data;
            }
            return null;
        }
    }
}
