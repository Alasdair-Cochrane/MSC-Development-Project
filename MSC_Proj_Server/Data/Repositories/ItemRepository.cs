using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class ItemRepository : Repository<Item> , IItemRepository
    {
        public ItemRepository(MainDbContext context) : base(context) { }
        
        public override async Task<Item?> GetAsync(int id)
        {
           var item =  await _context.Items.
                AsNoTracking().
                Where(x => x.Id == id).
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.EquipmentModel.Category).
                FirstOrDefaultAsync();

            return item;
        }        
        
        public override async Task<IEnumerable<Item>> GetAllAsync()
        {
            var entities = await _context.Items.
                AsNoTracking().
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.EquipmentModel.Category).
                ToListAsync();
            return entities;
        }

        public override async Task<Item?> AddAsync(Item item)
        {
            item.DateCreated= DateTime.UtcNow;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var added = await _context.Items.
                AsNoTracking().
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.EquipmentModel.Category).
                FirstOrDefaultAsync(x => x.Id == item.Id);
            return added;
        }

        public async Task<IEnumerable<Item>> AddManyAsync(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                item.DateCreated = DateTime.UtcNow;
            }
            _context.Items.AddRange(items);
            await _context.SaveChangesAsync();
            return items.Select(x => new Item
            {
                Id = x.Id,
                UnitId = x.UnitId,
                ModelId = x.ModelId,
                SerialNumber = x.SerialNumber,
                ItemStatusCategoryId = x.ItemStatusCategoryId,
                LocalName = x.LocalName,
                EquipmentModel = x.EquipmentModel,
                Condition_On_Reciept = x.Condition_On_Reciept,
                
            });
        }

        public override async Task<Item?> UpdateAsync(Item item)
        { 
             _context.Items.Update(item);
            await _context.SaveChangesAsync();
            var added = await _context.Items.
                AsNoTracking().
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                Include(x => x.Notes).
                Include(x => x.StatusCategory).
                Include(x => x.EquipmentModel.Category).
                FirstOrDefaultAsync(x => x.Id == item.Id);
            return added;
        }
        public async Task<IEnumerable<Item>> GetAllByUnitIdAsync(int unitId)
        {
            var items = await _context.Items.
                AsNoTracking().
                Where(x => x.UnitId == unitId)
                .Include(x => x.EquipmentModel).
                Include(x => x.StatusCategory).
                ToListAsync();
            return items;
        }

        public async Task<IEnumerable<Item>> GetAllByUnitIdAsync(IEnumerable<int> unitIds)
        {
            var items = await _context.Items.
                AsNoTracking().
                Where(x => unitIds.Contains(x.UnitId)).
                Include(x => x.EquipmentModel).
                Include(x => x.StatusCategory).
                ToListAsync();
            return items;
        }

        public async Task<bool> SetImageUrlAsync(int id, string url)
        {
            int updates = await _context.Items.Where(x => x.Id == id).
                ExecuteUpdateAsync(i =>
                i.SetProperty(f =>
                f.Image, url));
            if(updates > 0) { return true; }
            return false;
        }

        public async Task<string?> GetImageUrl(int id)
        {
            string? url = await _context.Items.
                Where(x => x.Id == id).
                Select(x => x.Image).
                SingleAsync();
            return url;
        }

        public async Task<Document> AddDocument(Document document)
        {
            var doc = await _context.Documents.AddAsync(document);
            await SaveAsync();
            return doc.Entity;
        }

        public async Task<IEnumerable<ItemExportDTO>> GetExportData(IEnumerable<int> unitIds)
        {
            var items = await _context.Items.
                Where(x => unitIds.Contains(x.UnitId)).

                Select(item => new ItemExportDTO
                {
                    Id = item.Id,
                    SerialNumber = item.SerialNumber,
                    LocalName = item.LocalName ?? "",
                    Barcode = item.Barcode ?? "",
                    Status = item.StatusCategory.Name,

                    ModelName = item.EquipmentModel.ModelName,
                    ModelNumber = item.EquipmentModel.ModelNumber,
                    Manufacturer = item.EquipmentModel.Manufacturer,
                    Category = item.EquipmentModel.Category.Name,

                    UnitId = item.UnitId,
                    UnitName = item.Unit.Name,
                    Building = item.Unit.Building,
                    Room = item.Unit.Room,
                    Address = item.Unit.Address,

                    Date_of_reciept = item.Date_Of_Reciept,
                    Date_of_commissioning = item.Date_Of_Commissioning,
                    Condition_on_reciept = item.Condition_On_Reciept,
                }).
                ToListAsync();
            return items;

        }

        public async Task UpdateImageUrl(int id, string url)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                throw new DataInsertionException($"Could not find item {id} to update image");
            }
            item.Image = url;
            _context.SaveChanges();
        }

        public async Task<IEnumerable<StatusQuantity>> GetQuantityByStatusAsync(IEnumerable<int> unitIds)
        {
            var categories = await _context.Items.
                Where(x => unitIds.Contains(x.UnitId)).
                GroupBy(x => x.ItemStatusCategoryId).
                Select(y => new StatusQuantity
                {
                    ItemQuantity = y.Count(),
                    StatusId = y.Key,
                    StatusName = y.First().StatusCategory.Name,
                    HexColor = y.First().StatusCategory.ColorHex,
                    Order = y.First().StatusCategory.Order,
                }).
                OrderBy(x => x.Order).
                ToListAsync();
            
            return categories;
        }

        public async Task<IEnumerable<Item>> GetLatestCreatedItemsAsync(int daysBeforeNow, IEnumerable<int> unitIds)
        {
            var dateBefore = DateTime.UtcNow.AddDays(-daysBeforeNow);
            var items = await _context.Items.
                Where(x => x.DateCreated.Date >= dateBefore.Date).
                Where(x => unitIds.Contains(x.UnitId)).
                Include(x => x.Unit).
                Include(x => x.EquipmentModel).
                ThenInclude(x => x.Category).
                Include(x => x.StatusCategory).
                OrderByDescending(x => x.DateCreated).
                ToListAsync();
            return items;
        }

    }
}
