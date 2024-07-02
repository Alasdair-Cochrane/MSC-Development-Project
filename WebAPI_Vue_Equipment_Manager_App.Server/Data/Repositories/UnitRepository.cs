using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        public UnitRepository(MainDbContext context) : base(context)
        {
        }

       public async Task<int> FindByName(string name)
        {
            var unit = await _context.Units.
                Where(x => x.Name.StartsWith(name)).
                SingleOrDefaultAsync();
            if(unit == null)
            {
                return -1;
            }
            return unit.Id;
        }
    }
}
