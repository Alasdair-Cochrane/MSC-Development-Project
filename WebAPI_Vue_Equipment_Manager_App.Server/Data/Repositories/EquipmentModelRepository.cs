        using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class EquipmentModelRepository : Repository<EquipmentModel>, IEquipmentModelRepository
    {
  
        public EquipmentModelRepository(MainDbContext context) : base(context)
        {
        }

        public async Task<EquipmentModel?> GetWithNavPropertiesAsync(int id)
        {
            var found = await _context.Models.
                AsNoTracking().
                Where(x => x.Id == id).
                Include(x => x.Category).
                FirstOrDefaultAsync();                

            return found;
        }

        public async Task<IEnumerable<EquipmentModel>> GetAllWithNavPropertiesAsync()
        {
            return await _context.Models.AsQueryable().
                Include(x => x.Category).
                ToListAsync();
        }
       
    }
}
