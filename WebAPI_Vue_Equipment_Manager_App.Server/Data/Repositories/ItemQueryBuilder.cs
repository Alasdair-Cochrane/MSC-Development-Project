using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;


//For case insensitive searching https://stackoverflow.com/questions/43277868/entity-framework-core-contains-is-case-sensitive-or-case-insensitive
namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class ItemQueryBuilder : IItemQueryBuilder
    {

        private readonly MainDbContext _context;
        private readonly IUnitRepository _unitRepository;

        public ItemQueryBuilder(MainDbContext mainDbContext, IUnitRepository unitRepository)
        {
            _context = mainDbContext;
            _unitRepository = unitRepository;
        }
        public async Task<IEnumerable<Item>?> QueryItems(ItemQuery queryObject, int userId)
        {
            //get all the unit ids for which the user is assigned as private or admin (or their child units)
            var units = await _unitRepository.GetAllRelevantUnitsToUserAsync(userId);
            var unitIds = units.Select(x => x.Id).ToList(); 

            IQueryable<Item> query = _context.Items.AsQueryable();

            //apply the filter on only those items belonging to the assigned units
            query = query.Where(x => unitIds.Any(y => y == x.UnitId));

            if (!String.IsNullOrEmpty(queryObject.SerialNumber))
            {
                query = query.Where(x => EF.Functions.ILike(x.SerialNumber, $"%{queryObject.SerialNumber}%"));
            }

            if (!String.IsNullOrEmpty(queryObject.LocalName))
            {
                query = query.Where(x => x.LocalName != null).
                    Where(x => EF.Functions.ILike(x.LocalName!, $"%{queryObject.LocalName}%"));
            }

            if (!String.IsNullOrEmpty(queryObject.Barcode))
            {
                query = query.Where(x => EF.Functions.ILike(x.Barcode, $"%{queryObject.Barcode}%"));
            }
            if(queryObject.StatusCategoryId != null)
            {
                query = query.Where(x => x.ItemStatusCategoryId == queryObject.StatusCategoryId);
            }

            if (!String.IsNullOrEmpty(queryObject.UnitName))
            {
                Unit? unit = units.
                    Where(x => x.Name.ToLower().Contains(queryObject.UnitName.ToLower())).
                    FirstOrDefault();
                if (unit != null )
                {
                    query = query.Where(x => x.UnitId == unit.Id);
                }
                else
                {
                   return Enumerable.Empty<Item>();
                }
            }
            if (queryObject.UnitId != null)
            {
                query = query.Where(x => x.UnitId == queryObject.UnitId);
            }

            if (!String.IsNullOrEmpty(queryObject.ModelName) ||
                    !String.IsNullOrEmpty(queryObject.Manufacturer) ||
                    !String.IsNullOrEmpty(queryObject.Category) ||
                    !String.IsNullOrEmpty(queryObject.ModelNumber) )
            {

                HashSet<int> modelIds = new HashSet<int>();

                if (!String.IsNullOrEmpty(queryObject.ModelNumber))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => EF.Functions.ILike(x.ModelNumber, $"%{queryObject.ModelNumber}%")).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.ModelName))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => EF.Functions.ILike(x.ModelName, $"%{queryObject.ModelName}%")).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.Manufacturer))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => EF.Functions.ILike(x.Manufacturer, $"%{queryObject.Manufacturer}%")).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.Category))
                {
                    modelIds.UnionWith(
                        await _context.Models.
                        Where(x => EF.Functions.ILike(x.Category.Name, $"%{queryObject.Category}%")).
                        Select(x => x.Id).
                        ToListAsync()
                        );
                }
                if (!modelIds.IsNullOrEmpty())
                {
                    query = query.Where(x => modelIds.Contains(x.ModelId));

                }
                else
                {
                   return Enumerable.Empty<Item>();
                }
            }

            List<Item> results = await query.
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                ThenInclude(x => x.Category).
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.Documents).
                ThenInclude(x => x.Document).
                Include(x => x.Maintenances).
                ThenInclude(x => x.Documents).
                Include(x => x.Maintenances).
                ThenInclude(x => x.Category). 
                Include(x => x.Notes).
                AsSplitQuery().
                AsNoTracking().
                ToListAsync();
            return results;
        }

    }
}
