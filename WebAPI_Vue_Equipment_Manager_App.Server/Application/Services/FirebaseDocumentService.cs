using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System.IO.Compression;
using System.Reflection;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;
using WebAPI_Vue_Equipment_Manager_App.Server.Startup;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    //File service for retrieving adding/retrieving/deleting files to Firebase cloud
    public class FirebaseDocumentService : IDocumentService
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;
        private readonly DocumentRepository _documentRepository;

        public FirebaseDocumentService(StorageClientProvider clientProvider, DocumentRepository docRepo)
        {
            _storageClient = clientProvider.GetClient();
            _bucketName = "msc-development-project-2da37.appspot.com";
            _documentRepository = docRepo;
        }

        public async Task<byte[]?> RetrieveAsync(string uri)
        {
           using(var stream = new  MemoryStream()) {
                var result = await _storageClient.DownloadObjectAsync(_bucketName, uri, stream);
                return stream.ToArray();
            }
        }

        public async Task UploadAsync( IFormFile file, string uri)
        {           
           await _storageClient.UploadObjectAsync(_bucketName, uri, null, file.OpenReadStream());
        }

        //Not Implemented - intended for file size validation/ virus scanning
        public async Task<bool> ValidateAsync(IFormFile file)
        {
            var result = true;
           return result;
        }

        public async Task RemoveAsync(string uri, bool andRecord) { 

            await _storageClient.DeleteObjectAsync(_bucketName, uri);
            if (andRecord)
            {
                await _documentRepository.DeleteDocument(uri);

            }
        }
    }

    
}
