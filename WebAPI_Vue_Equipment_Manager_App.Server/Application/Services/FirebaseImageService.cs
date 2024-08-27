
using Google.Cloud.Storage.V1;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;
using WebAPI_Vue_Equipment_Manager_App.Server.Startup;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    //For adding/retrieving images from FireBase Cloud
    public class FirebaseImageService : IImageService
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public FirebaseImageService(StorageClientProvider provider)
        {
            _storageClient = provider.GetClient();
            _bucketName = "msc-development-project-2da37.appspot.com";
        }

        public async Task<byte[]?> Retrieve(string fileName)
        {
            using(var stream = new  MemoryStream())
            {
                var image = await _storageClient.DownloadObjectAsync(_bucketName,fileName,stream);
                return stream.ToArray();
            }
        }

        public async Task Upload(IFormFile file, string fileName)
        {
            await _storageClient.UploadObjectAsync(_bucketName,fileName,null,file.OpenReadStream());
        }
    }
}
