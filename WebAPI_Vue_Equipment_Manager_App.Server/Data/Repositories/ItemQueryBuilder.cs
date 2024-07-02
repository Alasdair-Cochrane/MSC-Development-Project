using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Filters;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class ItemQueryBuilder : IItemQueryBuilder
    {

        private readonly MainDbContext _context;

        public ItemQueryBuilder(MainDbContext mainDbContext)
        {
            _context = mainDbContext;
        }

        public async Task<IEnumerable<Item>?> QueryItems(ItemQuery queryObject)
        {
            IQueryable<Item> query = _context.Items.AsQueryable();

            if (!queryObject.SerialNumber.IsNullOrEmpty())
            {
                query = query.Where(x => x.SerialNumber.StartsWith(queryObject.SerialNumber));
            }

            if (!queryObject.LocalName.IsNullOrEmpty())
            {
                query = query.Where(x => x.LocalName != null).
                    Where(x => x.LocalName.StartsWith(queryObject.LocalName));
            }

            if (!queryObject.Barcode.IsNullOrEmpty())
            {
                query = query.Where(x => x.Barcode == (queryObject.Barcode));
            }

            if (!queryObject.UnitName.IsNullOrEmpty())
            {
                Unit? unit = await _context.Units.
                    Where(x => x.Name.StartsWith(queryObject.UnitName)).
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

            if (!queryObject.ModelName.IsNullOrEmpty() ||
                    !queryObject.Manufacturer.IsNullOrEmpty() ||
                    queryObject.Category.IsNullOrEmpty() ||
                    queryObject.ModelName.IsNullOrEmpty())
            {

                HashSet<int> modelIds = new HashSet<int>();

                if (!queryObject.ModelNumber.IsNullOrEmpty())
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.Model_Number.StartsWith(queryObject.ModelNumber)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!queryObject.ModelName.IsNullOrEmpty())
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.Model_Name.StartsWith(queryObject.ModelName)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!queryObject.Manufacturer.IsNullOrEmpty())
                {
                    modelIds.UnionWith(await _context.Models.
                        Where(x => x.Manufacturer.StartsWith(queryObject.Manufacturer)).
                        Select(x => x.Id).
                        ToListAsync());
                }
                if (!queryObject.Category.IsNullOrEmpty())
                {
                    modelIds.UnionWith(
                        await _context.Models.
                        Where(x => x.Category.Name.StartsWith(queryObject.Category)).
                        Select(x => x.Id).
                        ToListAsync()
                        );
                }
                if (!modelIds.IsNullOrEmpty())
                {
                    query = query.Where(x => modelIds.Contains(x.Id));

                }
                else
                {
                   // return Enumerable.Empty<Item>();
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
