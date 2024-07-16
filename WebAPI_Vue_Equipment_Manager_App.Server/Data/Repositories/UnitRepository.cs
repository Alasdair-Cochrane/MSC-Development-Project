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

        //https://www.postgresqltutorial.com/postgresql-tutorial/postgresql-recursive-query/
        //Recursive CTEs are the most efficient way to recursively query the heirarchical relationship
        public async Task<IEnumerable<int>> GetChildrenIDsOfUnit(int parentId)
        {
            var query =
                "With Recursive children As (Select \"Id\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"ParentId\" From \"Units\" y Inner Join children c On c.\"Id\" = y.\"ParentId\") Select * From children";

            var children = await _context.Units.FromSqlRaw(query, parentId).Select(x => x.Id).ToListAsync();
            return children;
        }

        public async Task<IEnumerable<int>> GetParentIDsofUnit(int childId)
        {
            var query =
               "With Recursive parents As (Select \"Id\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents";

            var children = await _context.Units.FromSqlRaw(query, childId).Select(x => x.Id).ToListAsync();
            return children;
        }
    }
}
