using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
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
                Include(x => x.Category).
                FirstOrDefaultAsync(x => x.Id == id);   
            return found;
        }


        //
        public  async Task<IEnumerable<MaintenanceDTO>> GetAllInTimePeriodAsync(int days, IEnumerable<int> unitIds)
        {
            var dateNow = DateTime.UtcNow;
            var daysFromNow = dateNow.AddDays(-days);
            var list = await _context.Maintenances.
                AsNoTracking().
                Where(x => x.Date_Completed.Date >= daysFromNow.Date).
                Join(_context.Items, n => n.ItemId, i => i.Id, (n,i) => new 
                {
                    Category = n.Category,
                    Date_Completed = n.Date_Completed,
                    Description = n.Description,
                    Provider_Name = n.Provider_Name,
                    MaintenanceCategoryId = n.MaintenanceCategoryId,
                    ItemId = n.ItemId,
                    UnitId = i.UnitId,
                    SerialNumber = i.SerialNumber,
                    Id = n.Id,

                }).
                Where(x => unitIds.Contains(x.UnitId)).
                Select( n => new MaintenanceDTO
                {
                    CategoryName = n.Category.Name,
                    Date_Completed = n.Date_Completed,
                    Description = n.Description,
                    Provider_Name = n.Provider_Name,
                    ItemId = n.ItemId,
                    SerialNumber = n.SerialNumber,
                    Id = n.Id,
                }).
                OrderByDescending( x => x.Date_Completed).
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
                Include(x => x.Category).
                Where(x => x.ItemId == id).
                ToListAsync();
            return list;
        }

        public async Task<IEnumerable<string>> GetAllCategoryNamesAsync()
        {
            var list = await _context.MaintenanceTypes.Select(x => x.Name).ToListAsync();
            return list;
        }
        
    }
}
