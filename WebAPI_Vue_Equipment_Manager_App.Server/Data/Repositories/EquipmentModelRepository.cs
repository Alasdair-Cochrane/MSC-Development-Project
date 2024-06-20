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

        public override async Task<EquipmentModel?> AddAsync(EquipmentModel newModel)
        {
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();
            var model = await _context.Models.
                AsNoTracking().
                Include(x => x.Category).
                FirstOrDefaultAsync(x => x.Id == newModel.Id);
            return model;
        }

        public override async Task<EquipmentModel?> UpdateAsync(EquipmentModel toUpdate)
        {
            _context.Models.Update(toUpdate);
            await _context.SaveChangesAsync();
            var updated = await _context.Models.
                AsNoTracking().
                Include(x => x.Category).
                FirstOrDefaultAsync(x => x.Id == toUpdate.Id);
            return updated;

        }
       
    }
}
