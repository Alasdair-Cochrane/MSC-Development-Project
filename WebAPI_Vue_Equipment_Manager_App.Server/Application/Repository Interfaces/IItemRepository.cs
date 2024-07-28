using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        public Task<IEnumerable<Item>> GetAllByUnitIdAsync(int unitId);
        public Task<bool> SetImageUrlAsync(int id, string url);
        public Task<string?> GetImageUrl(int id);
        public Task<Document> AddDocument(Document item);
        Task<IEnumerable<Item>> GetAllByUnitIdAsync(IEnumerable<int> unitIds);
        Task<IEnumerable<ItemExportDTO>> GetExportData(IEnumerable<int> unitIds);
        Task UpdateImageUrl(int id, string url);
    }
}
