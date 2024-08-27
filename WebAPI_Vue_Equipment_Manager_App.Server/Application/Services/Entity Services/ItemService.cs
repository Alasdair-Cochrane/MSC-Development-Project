using CsvHelper;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly ICategoryRepository<ItemStatusCategory> _statusRepository;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IEquipmentModelRepository _modelRepository;
        private readonly ICategoryRepository<EquipmentModelCategory> _modelCategories;
        private readonly IUnitRepository _unitRepository;
        private readonly IItemQueryBuilder _itemQueryBuilder;
        private readonly IUserService _userService;
        private readonly DocumentRepository _documentRepository;
        public ItemService(IItemRepository itemRepository, ICategoryRepository<ItemStatusCategory> categories,
            IMaintenanceService maintenanceService, IEquipmentModelRepository modelRepository,
            ICategoryRepository<EquipmentModelCategory> modelCategories, IUnitRepository unitRepository,
            IItemQueryBuilder itemQueryBuilder,
            IUserService userService,
            DocumentRepository documentRepository)
        {
            _ItemRepository = itemRepository;
            _statusRepository = categories;
            _maintenanceService = maintenanceService;
            _modelRepository = modelRepository;
            _modelCategories = modelCategories;
            _unitRepository = unitRepository;
            _itemQueryBuilder = itemQueryBuilder;
            _userService = userService;
            _documentRepository = documentRepository;
        }

        public async Task<ItemDTO?> AddAsync(ItemDTO item, int userId)
        {
            //will return an HttpResponse if user is not authorised to view the unit's items
            await CheckIsAuthorisedAsync(item.UnitId.Value, userId);

            //Get the Foriegn Key Ids for the related status and model category entities
            int statusCategoryID = _statusRepository.FindOrCreateByName(item.CurrentStatus).Id;
            int modelCategoryID = _modelCategories.FindOrCreateByName(item.Model.Category).Id;

            //Either create or update the model associated with the posted item
            EquipmentModel? model = await _modelRepository.UpsertbyModelNumberAsync(item.Model.ToEntity(modelCategoryID));

            if (model == null)
            {
                string entity = JsonSerializer.Serialize(item);
                throw new DataInsertionException("failed to find or create model entry when adding item", entity);
            }

            Item newItem = item.ToEntity(statusCategoryID);
            newItem.ModelId = model.Id;
            var added = await _ItemRepository.AddAsync(newItem);

            if (added == null)
            {
                string entity = JsonSerializer.Serialize(item);

                throw new DataInsertionException("Failed to create item", entity);
            }
            return added.ToDTO();
        }


        public async Task DeleteAsync(int itemId, int userId)
        {
            var item = await _ItemRepository.GetAsync(itemId);
            if(item == null)
            {
                return;
            }
            await CheckIsAuthorisedAsync(item.UnitId, userId);

            await _ItemRepository.DeleteAsync(item.Id);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllAsync()
        {
            var all = await _ItemRepository.GetAllAsync();
            return all.Select(x => x.ToDTO());
        }

        public async Task<ItemDTO?> GetByIdAsync(int id)
        {

            var found = await _ItemRepository.GetAsync(id);
            if (found == null)
            {
                return null;
            }
            return found.ToDTO();
        }

        public async Task<ItemDTO?> UpdateAsync(ItemDTO item, int userId)
        {
            if(item.UnitId == null)
            {
                throw new UnauthorisedOperationException($"No unitID specified for item {item.Id}");
            }

            await CheckIsAuthorisedAsync(item.UnitId.Value, userId);

            int statusId = _statusRepository.FindOrCreateByName(item.CurrentStatus).Id;
            int modelCategoryID = _modelCategories.FindOrCreateByName(item.Model.Category).Id;

            //make sure any changes to the model are also tracked
            var model = await _modelRepository.UpsertbyModelNumberAsync(item.Model.ToEntity(modelCategoryID));
            item.ModelId = model.Id;

            var updated = await _ItemRepository.UpdateAsync(item.ToEntity(statusId));
            if (updated == null)
            {
                return null;
            }
            return updated.ToDTO();
        }
        public async Task<IEnumerable<ItemDTO>> GetAllByUnitIdAsync(int UnitID)
        {
            var list = await _ItemRepository.GetAllByUnitIdAsync(UnitID);
            return list.Select(x => x.ToDTO());

        }

        public async Task SetImageUrl(int itemId, string url)
        {
            await _ItemRepository.SetImageUrlAsync(itemId, url);
        }

        public async Task<string?> GetImageUrl(int id)
        {
            var url = await _ItemRepository.GetImageUrl(id);
            if (url == null) return null;
            return url;
        }

        public async Task<IEnumerable<ItemDTO>?> SearchAsync(ItemQuery query, int userId)
        {
            var items = await _itemQueryBuilder.QueryItems(query, userId);
            List<ItemDTO>? result = new List<ItemDTO>();

            if (items.IsNullOrEmpty())
            {
                return null;
            }
            foreach (Item item in items!)
            {
                result.Add(item.ToDTO());
            }

            return result;
        }

        //https://joshclose.github.io/CsvHelper/getting-started/#writing-a-csv-file Using 'CSV Helper' library
        //https://stackoverflow.com/questions/52741533/how-to-export-csv-file-from-asp-net-core
        public async Task<MemoryStream> GetExport(int userId, IEnumerable<int>? unitIds = null)
        {
        //make sure that the user is authorised to view items belonging to the requested units
            var relevantUnits = await _unitRepository.GetAllRelevantUnitsToUserAsync(userId, true);

            IEnumerable<int> relevantUnitIds = relevantUnits.Select(x => x.Id);

            if (!unitIds.IsNullOrEmpty())
            {
                foreach (int id in unitIds!)
                {
                    if (!relevantUnitIds.Contains(id))
                    {
                        throw new UnauthorisedOperationException($"User is not authorised to view items from Unit {id}");
                    }
                }
                relevantUnitIds = unitIds;
            }

            //Gets all items for those units as ExportData objects
            var exportData = await _ItemRepository.GetExportData(relevantUnitIds);

            var stream = new MemoryStream();
            using (var file = new StreamWriter(stream, leaveOpen: true))
            {
                var csv = new CsvWriter(file, CultureInfo.InvariantCulture);
                csv.WriteRecords(exportData);
            }
            stream.Position = 0;
            return stream;
        }

        //will throw an exception if user is unauthorised to view unit - otherwise operation will proceed
        private async Task CheckIsAuthorisedAsync(int unitId, int userId)
        {
            if(unitId == 0)
            {
                throw new UnauthorisedOperationException("UnitId not specified");
            }

            if (!await _userService.CheckUserIsPrivateOrAdminIncParent(userId, unitId))
            {

                throw new UnauthorisedOperationException($"UserID {userId} is notunauthorised for this operation for Unit {unitId} ");
            }
        }

        public async Task<ItemDocument> CreateItemDocumentAsync(Document document, int itemId)
        {
            var result = await _documentRepository.AddItemDocument(document, itemId);
            return result;
        }

        public async Task UpdateImageUrl(int id, string url)
        {
            await _ItemRepository.UpdateImageUrl(id, url);
        }

        public async Task<IEnumerable<string>> GetStatusCategoryNames()
        {
            var statuses = await _statusRepository.GetAllAsync();
            var names = statuses.Select(x => x.Name).ToList();
            return names;
        }

        public async Task<IEnumerable<StatusQuantity>> GetQuantityByStatusAsync(int userId)
        {
            var units = await _unitRepository.GetAllRelevantUnitIdsToUserAsync(userId);
            var result = await  _ItemRepository.GetQuantityByStatusAsync(units);
            return result;
        }

        public async Task<IEnumerable<ItemDTO>> GetLatestCreatedItems(int daysBefore, int userId)
        {
            var units = await _unitRepository.GetAllRelevantUnitIdsToUserAsync(userId,true);
            var items = await _ItemRepository.GetLatestCreatedItemsAsync(daysBefore, units);
            return items.Select(x => x.ToDTO());

        }


    }
    

 

    public interface IItemService
    {
        public Task<ItemDTO?> GetByIdAsync(int id);
        public Task<IEnumerable<ItemDTO>> GetAllAsync();
        public Task<ItemDTO?> UpdateAsync(ItemDTO item, int userId );
        public Task DeleteAsync(int itemId, int userId);
        public Task<ItemDTO?> AddAsync(ItemDTO item, int userId);
        public Task<IEnumerable<ItemDTO>> GetAllByUnitIdAsync(int UnitID);
        public Task SetImageUrl(int itemId, string url);
        public Task<string?> GetImageUrl(int id);
        public Task<IEnumerable<ItemDTO>?> SearchAsync(ItemQuery query, int userId);
        public Task<MemoryStream> GetExport(int userId, IEnumerable<int>? unitIds = null);
        public  Task<ItemDocument> CreateItemDocumentAsync(Document document, int itemId);
        public Task UpdateImageUrl(int id, string url);
        Task<IEnumerable<string>> GetStatusCategoryNames();
        Task<IEnumerable<StatusQuantity>> GetQuantityByStatusAsync(int userId);
        Task<IEnumerable<ItemDTO>> GetLatestCreatedItems(int daysBefore, int userId);
    }
}
