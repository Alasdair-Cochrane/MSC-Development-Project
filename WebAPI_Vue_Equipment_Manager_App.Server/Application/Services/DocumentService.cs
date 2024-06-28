using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly string _baseDirectory;
        private readonly IItemRepository _itemRepository;
        public DocumentService(IWebHostEnvironment env, IItemRepository items) {
            _baseDirectory = Path.Combine(env.WebRootPath, "documents");
            _itemRepository = items;
                
        }

        public async Task<byte[]?> RetrieveAsync(string fileName)
        {
            try
            {
                var file = await File.ReadAllBytesAsync(Path.Combine(_baseDirectory, fileName));
                return file;
            }
            catch
            {
                //should replace with specific execptions - filenot found exception
                return null;
            }
        }

        public async Task<ItemDocument> UploadAsync(int id, string url,  IFormFile file, string fileName)
        {
            string path = Path.Combine(_baseDirectory, fileName);
            try
            {
                using (var stream = File.Create(path))
                {

                    await file.CopyToAsync(stream);
                }
                var doc = await _itemRepository.AddDocument(new ItemDocument
                {
                    ItemId = id,
                    URL = url,
                });

                return doc;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> ValidateAsync(IFormFile file)
        {
            return await Task.FromResult(true);
        }
    }

    public interface IDocumentService
    { 
        public Task<bool> ValidateAsync(IFormFile file);
        public Task<ItemDocument> UploadAsync(int id, string url, IFormFile file, string fileName);
        public Task<byte[]?> RetrieveAsync(string filename);

    
    }

}
