using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class EquipmentModelRepository : Repository<EquipmentModel>, IEquipmentModelRepository
    {
  
        public EquipmentModelRepository(MainDbContext context) : base(context)
        {
        }

        public override async Task<EquipmentModel?> GetAsync(int id)
        {
            var found = await _context.Models.
                AsNoTracking().
                Where(x => x.Id == id).
                Include(x => x.Category).
                FirstOrDefaultAsync();                

            return found;
        }

        public override async Task<IEnumerable<EquipmentModel>> GetAllAsync()
        {
            return await _context.Models.AsQueryable().
                Include(x => x.Category).
                ToListAsync();
        }
       
    }
}
