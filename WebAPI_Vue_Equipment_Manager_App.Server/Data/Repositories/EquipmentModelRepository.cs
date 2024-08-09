        using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class EquipmentModelRepository : Repository<EquipmentModel>, IEquipmentModelRepository
    {
  
        public EquipmentModelRepository(MainDbContext context) : base(context)
        {
        }

        public override async Task<EquipmentModel?> GetAsync(int id)
        {
            var found = await _context.Models.
                AsNoTracking().
                Where(x => x.Id == id).
                Include(x => x.Category).
                FirstOrDefaultAsync();                

            return found;
        }

        public override async Task<IEnumerable<EquipmentModel>> GetAllAsync()
        {
            return await _context.Models.AsQueryable().
                Include(x => x.Category).
                ToListAsync();
        }

        public override async Task<EquipmentModel?> AddAsync(EquipmentModel newModel)
        {
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();
            var model = await _context.Models.
                AsNoTracking().
                Include(x => x.Category).
                FirstOrDefaultAsync(x => x.Id == newModel.Id);
            return model;
        }

        //used just to populate data in the database for testing purposes
        ///makes sure not to add if already exists(determined by model number)
        public async Task AddManyAsync(IEnumerable<EquipmentModel> models)
        {
            var existing = models.Where(x => _context.Models.Select(x => x.ModelNumber).Any(m => m == x.ModelNumber)).
                Select(x => x.ModelNumber).
                ToList();
            _context.Models.AddRange(models.ExceptBy(existing, x => x.ModelNumber));
            await _context.SaveChangesAsync();
        }

        public override async Task<EquipmentModel> UpdateAsync(EquipmentModel toUpdate)
        {
           _context.Models.Update(toUpdate);
            await _context.SaveChangesAsync();
            var updated = await _context.Models.
                AsNoTracking().
                Include(x => x.Category).
                FirstOrDefaultAsync(x => x.Id == toUpdate.Id);
            if(updated == null)
            {
                throw new DataInsertionException($"Could not update model : {JsonSerializer.Serialize(toUpdate)}");
            }
            return updated;

        }

        public async Task<EquipmentModel> UpsertbyModelNumberAsync(EquipmentModel model)
        {
            //find the existing entry that matches either the given id, modelname or model number
            //if it exists then assign its id to model the update model so that any changes are persisted
            var found = await _context.Models.
                    Where(x => x.Id == model.Id || x.ModelNumber == model.ModelNumber && x.Manufacturer == model.Manufacturer
                    ).AsNoTracking().
                    FirstOrDefaultAsync();                
            if(found != null)
            {
                model.Id = found.Id;
            }

            //update rather than add so that changes to model can be tracked
            return await UpdateAsync(model);                   
        }

        public async Task<IEnumerable<string>> GetUserCategoriesAsync(IEnumerable<int> unitIds)
        {
            var categories = await _context.Items.
                Where(x => unitIds.Contains(x.UnitId)).
                Select(x => x.EquipmentModel.Category.Name).
                ToListAsync();
            return categories;
        }

    }
}
