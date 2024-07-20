using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        public UnitRepository(MainDbContext context) : base(context)
        {
        }

       public async Task<Unit?> FindByName(string name)
        {
            var unit = await _context.Units.
                Where(x => x.Name == name).
                SingleOrDefaultAsync();
            if (unit == null)
            {
                return null;
            }
            return unit;
        }

       
        public async Task<IEnumerable<UserDetailsDTO>> GetUsersInUnit(int unitId)
        {
            var users = await _context.Assignments.Where(x => x.UnitId == unitId).
                Select(x => new UserDetailsDTO
                {
                    Id = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email ?? "",
                }).ToListAsync();

            return users;
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
               "With Recursive parents As (Select \"Id\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select  From parents";

            var parents = await _context.Units.FromSqlRaw(query, childId).Select(x => x.Id).ToListAsync();
            return parents;
        }

        public async Task<IEnumerable<Unit>> GetChildrenofUnit(int parentId)
        {
            var query =
                "With Recursive children As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join children c On c.\"Id\" = y.\"ParentId\") Select * From children";

            var children = await _context.Units.FromSqlRaw(query, parentId).ToListAsync();
            return children;
        }

        public async Task<IEnumerable<Unit>> GetParentsofUnit(int childId)
        {
            var query =
               "With Recursive parents As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents";

            var parents = await _context.Units.FromSqlRaw(query, childId).ToListAsync();
            return parents;
        }

        public async Task<IEnumerable<Unit>> GetAllTopLevelUnitsAsync()
        {
            var units = await _context.Units.
                Where(x => x.ParentId == null).
                ToListAsync();
            return units;
        }
        public async Task<bool> CheckUserHasRoleInUnit(int userId, int roleId, int unitId)
        {
            var assignment = await _context.Assignments.AllAsync(x =>
            x.UserId == userId &&
            x.UnitId == unitId &&
            x.RoleId == roleId);
            return assignment;
        }

        //checks whether a user is an admin of the unit or of any of the unit's parents
        public async Task<bool> CheckUserIsAdminInParentOfUnit(int userId, int unitId)
        {
            var adminRoleUnits = await GetUnitIdsWithUserRole(userId, 1);

            if (adminRoleUnits.IsNullOrEmpty()) { return false; }

            var children = await GetParentIDsofUnit(unitId);

            return children.Any(x => adminRoleUnits.Contains(x));

        }

        //Gets all the units that the user has been assigned to and all their children
        public async Task<IEnumerable<Unit>> GetAllRelevantUnitsToUser(int userId)
        {
            List<UserAssignment> assignedUnits = await _context.Assignments.
                 AsNoTracking().
                 Where(x => x.UserId == userId).
                 Include(x => x.Unit).
                ToListAsync();
            
            HashSet<Unit> units = new HashSet<Unit>();
            HashSet<Unit> children = new HashSet<Unit>();
            units.UnionWith(assignedUnits.Select(x => x.Unit).ToList());

            foreach (var unit in units) 
            {
                children.UnionWith(await GetChildrenofUnit(unit.Id));
            }
            units.UnionWith(children);
            return units;
        }

        //Gets all units where the user has admin priviledges
        public async Task<IEnumerable<Unit>> GetAllAdminRoleUnits(int userId)
        {
            List<UserAssignment> assignedUnits = await _context.Assignments.
                 AsNoTracking().
                 Where(x => x.UserId == userId).
                 Where(x => x.RoleId == 1).
                 Include(x => x.Unit).
                ToListAsync();

            HashSet<Unit> units = new HashSet<Unit>();
            units.UnionWith(assignedUnits.Select(x => x.Unit).ToList());

            foreach (var unit in units)
            {
                units.UnionWith(await GetChildrenofUnit(unit.Id));
            }
            return units;
        }


        //Returns those units where the specified user has been assigned the specified role
        public async Task<IEnumerable<int>> GetUnitIdsWithUserRole(int userId, int roleId)
        {
            var units = await _context.Assignments.
                Where(x => x.UserId == userId && x.RoleId == roleId).
                Select(x => x.UnitId).
                ToListAsync();
            return units;
        }


        public async Task<IEnumerable<int>> GetAllUnitIdsWhereUserIsAdmin(int userId)
        {
            var unitIds = new HashSet<int>();
            var adminUnits = await GetUnitIdsWithUserRole(userId, 1);

            foreach( int unit in adminUnits)
            {
                unitIds.UnionWith(await GetChildrenIDsOfUnit(unit));   
            }
            return unitIds;
        }
    }
}
