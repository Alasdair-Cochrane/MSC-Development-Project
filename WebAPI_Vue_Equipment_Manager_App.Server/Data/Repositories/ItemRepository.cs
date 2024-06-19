using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class ItemRepository : Repository<Item> , IItemRepository
    {
        public ItemRepository(MainDbContext context) : base(context) { }
        public async Task<IEnumerable<Item>> GetAllWithNavPropertiesAsync()
        {
            var entities = await _context.Items.AsQueryable().
                AsNoTracking().
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                ToListAsync();
            return entities;
        }
        public async Task<Item?> GetWithNavPropertiesAsync(int id)
        {
            var found = await _context.Items.
                AsNoTracking().
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                FirstOrDefaultAsync();
            return found;
        }
    }
}
