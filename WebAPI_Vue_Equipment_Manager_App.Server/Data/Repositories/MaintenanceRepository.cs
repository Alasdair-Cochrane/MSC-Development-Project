using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class MaintenanceRepository : Repository<Maintenance> , IMaintenanceRepository
    {

        public MaintenanceRepository(MainDbContext context) : base(context) { }
        public override async Task<Maintenance?> GetAsync(int id)
        {
            var found = await _context.Maintenances.
                AsNoTracking().
                Include(x => x.Type).
                FirstOrDefaultAsync(x => x.Id == id);   
            return found;
        }

        public override async Task<IEnumerable<Maintenance>> GetAllAsync()
        {
            var list = await _context.Maintenances.
                AsNoTracking().
                Include(x => x.Type).
                ToListAsync();
            return list;
        }

        public override async Task<Maintenance?> UpdateAsync(Maintenance toUpdate)
        {
            _context.Maintenances.Update(toUpdate);
            await _context.SaveChangesAsync();
            var updated = await GetAsync(toUpdate.Id);
            return updated;
        }

        public override async Task<Maintenance?> AddAsync(Maintenance toAdd)
        {
            _context.Maintenances.Add(toAdd);
            await _context.SaveChangesAsync();
            var updated = await GetAsync(toAdd.Id);
            return updated;
        }

        public async Task<IEnumerable<Maintenance>> GetAllByItemIdAsync(int id)
        {
            var list = await _context.Maintenances.
                AsNoTracking().
                Include(x => x.Type).
                Where(x => x.TypeId == id).
                ToListAsync();
            return list;
        }
    }
}
