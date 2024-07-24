using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Filters;

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
                query = query.Where(x => x.SerialNumber.Contains(queryObject.SerialNumber!));
            }

            if (!String.IsNullOrEmpty(queryObject.LocalName))
            {
                query = query.Where(x => x.LocalName != null).
                    Where(x => x.LocalName.Contains(queryObject.LocalName!));
            }

            if (!String.IsNullOrEmpty(queryObject.Barcode))
            {
                query = query.Where(x => x.Barcode.Equals(queryObject.Barcode));
            }

            if (!String.IsNullOrEmpty(queryObject.UnitName))
            {
                Unit? unit = await _context.Units.
                    Where(x => x.Name.Contains(queryObject.UnitName!)).
                    FirstOrDefaultAsync();
                if (unit != null)
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
                    !String.IsNullOrEmpty(queryObject.ModelNumber))
            {

                HashSet<int> modelIds = new HashSet<int>();

                if (!String.IsNullOrEmpty(queryObject.ModelNumber))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.ModelNumber.Contains(queryObject.ModelNumber!)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.ModelName))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.ModelName.Contains(queryObject.ModelName!)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.Manufacturer))
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.Manufacturer.Contains(queryObject.Manufacturer!)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!String.IsNullOrEmpty(queryObject.Category))
                {
                    modelIds.UnionWith(
                        await _context.Models.
                        Where(x => x.Category.Name.Contains(queryObject.Category!)).
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
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.EquipmentModel.Category).
                ToListAsync();
            return results;
        }

    }
}
