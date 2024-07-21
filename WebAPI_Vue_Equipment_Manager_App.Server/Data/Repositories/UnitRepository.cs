using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        private readonly IAssignmentRepository _assignmentRepository;
        public UnitRepository(MainDbContext context, IAssignmentRepository assignmentRepository) : base(context)
        {
            _assignmentRepository = assignmentRepository;
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
               "With Recursive parents As (Select \"Id\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents";

            var parents = await _context.Units.FromSqlRaw(query, childId).Select(x => x.Id).ToListAsync();
            return parents;
        }

        public async Task<IEnumerable<Unit>> GetChildrenofUnit(int parentId)
        {
            var query =
                "With Recursive children As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join children c On c.\"Id\" = y.\"ParentId\") Select * From children";

            var children = await _context.Units.FromSqlRaw(query, parentId).Include(x => x.Children).
                ToListAsync();
            return children;
        }

        public async Task<IEnumerable<Unit>> GetParentsofUnit(int childId)
        {
            var query =
               "With Recursive parents As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents";

            var parents = await _context.Units.FromSqlRaw(query, childId).ToListAsync();
            return parents;
        }

        public async Task<Unit?> GetRootUnitAsync(int unitId)
        {
            var query = "With Recursive root As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents Where \"ParentId\" Is null";
            var root = await _context.Units.FromSqlRaw(query, unitId).Include(x => x.Children).FirstOrDefaultAsync();
            return root;
        }

        public async Task<Unit?> GetOrgStructure(int unitId)
        {
            var query = "With Recursive root As (Select \"Id\", \"Name\", \"Building\", \"Room\", \"Address\", \"ParentId\" From \"Units\" Where \"Id\" = {0} Union Select y.\"Id\", y.\"Name\", y.\"Building\", y.\"Room\", y.\"Address\", y.\"ParentId\" From \"Units\" y Inner Join parents p On y.\"Id\" = p.\"ParentId\") Select * From parents Where \"ParentId\" Is null";
            var root = await _context.Units.FromSqlRaw(query, unitId).
                Include(x => x.Children).
                FirstOrDefaultAsync();
            return root;
        }

        public async Task<IEnumerable<Unit>> GetAllRootUnitsAsync()
        {
            var units = await _context.Units.
                Where(x => x.ParentId == null).
                ToListAsync();
            return units;
        }


        //checks whether a user is an admin of the unit or of any of the unit's parents
        public async Task<bool> CheckUserIsAdminInParentOfUnit(int userId, int unitId)
        {
            var adminRoleUnits = await GetUnitIdsWithUserRole(userId, 1);

            if (adminRoleUnits.IsNullOrEmpty()) { return false; }

            var children = await GetParentIDsofUnit(unitId);

            return children.Any(x => adminRoleUnits.Contains(x));

        }

        //Gets all the units within the organisation that the user has been assigned to
        public async Task<IEnumerable<Unit>> GetAllRelevantUnitsToUserAsync(int userId)
        {
            List<UserAssignment> assignedUnits = await _context.Assignments.
                 AsNoTracking().
                 Where(x => x.UserId == userId).
                 Include(x => x.Unit).
                ToListAsync();

            HashSet<Unit> units = new HashSet<Unit>(assignedUnits.Select(x => x.Unit).ToList());

            foreach (var unit in assignedUnits.Where(x => x.RoleId == 1).Select(x => x.Unit).ToList())
            {
                units.UnionWith(await GetChildrenofUnit(unit.Id));
            }
            return units;
        }

        public async Task<IEnumerable<Unit>> GetAllAssignedRootsAsync(int userId)
        {
            var roots = await _context.Assignments.
                Where(x => x.UserId == userId).
                Where(x => x.Unit.ParentId == null).
                Include(x => x.Unit).
                ThenInclude(x => x.Children).
                Select(x => x.Unit)
                .ToListAsync();
            return roots;
        }

        public async Task<UnitDTO> GetDTOWithChildrenAsync(int UnitId)
        {
            Unit? unit = await _context.Units.Include(x => x.Children).
                Where(x => x.Id == UnitId).
                FirstOrDefaultAsync();
            if (unit == null)
            {
                throw new UnitException($"Could not find unit with id {UnitId}");
            }
            var dto = unit.ToDTO();
            dto.AssigedUsers = await GetUserAssignmentsForUnitAsync(unit.Id);
            if(unit.Children == null)
            {
                return dto;
            }
            List<UnitDTO> children = new List<UnitDTO>();

            foreach(var child in unit.Children)
            {
                children.Add(await GetDTOWithChildrenAsync(child.Id));
            }
            dto.Children = children;
            return dto;
        }


        public async Task<UnitDTO> GetOrgStructureAsync(int anyUnitId)
        {
            Unit? root = await GetRootUnitAsync(anyUnitId);
            if(root == null)
            {
                throw new UnitException($"Could not find root organisation for unit {anyUnitId}");
            }
            var dto = await GetDTOWithChildrenAsync(root.Id);
            return dto;
        }


        //Gets all units where the user has admin priviledges
        public async Task<IEnumerable<UnitDTO>> GetAllAdminRoleUnits(int userId)
        {
            List<int> adminUnits = await _context.Assignments.
                 AsNoTracking().
                 Where(x => x.UserId == userId).
                 Where(x => x.RoleId == 1).
                 Select(x => x.UnitId).
                ToListAsync();

            List<UnitDTO> units = new List<UnitDTO>();

            foreach (var unit in adminUnits)
            {
                units.Add(await GetDTOWithChildrenAsync(unit));
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

        public async Task<bool> CheckUserIsAssigned(int userId, int unitId)
        {
            bool result = await _context.Assignments.AnyAsync(x => x.UserId == userId && x.UnitId == unitId);
            return result;
        }

        public async Task<IEnumerable<int>> GetUserRolesInUnit(int userId, int unitId)
        {
            var roles = await _context.Assignments.Where(x => x.UserId == userId).
                Select(x => x.RoleId).ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<User>> GetAllAssignedUsersAsync(int unitId)
        {
            var users = await _context.Assignments.
                AsSplitQuery().
                Where(x => x.UnitId == unitId).
                Include(x => x.User).
                ThenInclude(x => x.Assignments).
                Select(x => x.User).
                ToListAsync();
            return users;
        }
        public async Task<bool> CheckUserHasRoleInUnit(int userId, int roleId, int unitId)
        {
            var assignment = await _context.Assignments.AllAsync(x =>
            x.UserId == userId &&
            x.UnitId == unitId &&
            x.RoleId == roleId);
            return assignment;
        }
        public async Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsForUnitAsync(int unitId)
        {
            var assignments = await _context.Assignments.
                Where(x => x.UnitId == unitId).
                AsSplitQuery().
                AsNoTracking().
                Include(x => x.User).
                Include(x => x.Unit).
                Include(x => x.Role).
                Select(x => new AssignmentDTO
                {
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    UnitName = x.Unit.Name,
                    RoleName = x.Role.Name,
                }).ToListAsync();
            return assignments;
        }
    }
}
