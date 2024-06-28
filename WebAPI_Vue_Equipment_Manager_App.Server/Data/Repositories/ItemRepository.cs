using Microsoft.EntityFrameworkCore;
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

        public async Task<ItemDocument> AddDocument(ItemDocument document)
        {
            var doc = await _context.ItemDocuments.AddAsync(document);
            await SaveAsync();
            return doc.Entity;
        }

    }
}
