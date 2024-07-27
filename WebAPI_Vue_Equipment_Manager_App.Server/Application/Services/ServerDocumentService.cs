using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class ServerDocumentService : IDocumentService
    {
        private readonly string _baseDirectory;
        private readonly IItemRepository _itemRepository;

        public ServerDocumentService(IWebHostEnvironment env, IItemRepository items) {
            _baseDirectory = Path.Combine(env.WebRootPath, "documents");
            _itemRepository = items;
                
        }

        public async Task<byte[]?> RetrieveAsync(string uri)
        {
            try
            {
                var file = await File.ReadAllBytesAsync(Path.Combine(_baseDirectory, uri));
                return file;
            }
            catch
            {
                //should replace with specific execptions - filenot found exception
                return null;
            }
        }

        public async Task UploadAsync(IFormFile file, string uri)
        {
            string path = Path.Combine(_baseDirectory, uri);
        
                using (var stream = File.Create(path))
                {

                    await file.CopyToAsync(stream);
                }    
        }

        public async Task<bool> ValidateAsync(IFormFile file)
        {
            return await Task.FromResult(true);
        }

        public async Task RemoveAsync(string uri)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDocumentService
    { 
        public Task<bool> ValidateAsync(IFormFile file);
        public Task UploadAsync(IFormFile file, string uri);
        public Task<byte[]?> RetrieveAsync(string filename);
        public Task RemoveAsync(string uri);

    
    }

}
