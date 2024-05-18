using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repositories.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DataContext _context;
        public LocationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Location> UpdateStatus(int id, int isActive)
        {
            var data = await _context.Locations.FindAsync(id);
            if (data != null)
            {
                data.IsActive = isActive;
                var result = _context.Locations.Update(data);
                await _context.SaveChangesAsync();
                return data;
            }
            return null;
        }
    }
}
