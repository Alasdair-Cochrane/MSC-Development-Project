using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System.IO.Compression;
using System.Reflection;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
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
        public async Task<Dictionary<string,byte[]>> RetrieveAll(IEnumerable<string> uris)
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            Dictionary<string, MemoryStream> streams = new Dictionary<string, MemoryStream>();
            List<Task<Google.Apis.Storage.v1.Data.Object>> tasks = new List<Task<Google.Apis.Storage.v1.Data.Object>>();

            foreach(var file in uris)
            {
                var stream = new MemoryStream();                
                tasks.Add(_storageClient.DownloadObjectAsync(_bucketName, file, stream));   
                streams.Add(file,stream);
            }
             await Task.WhenAll(tasks);

            foreach(var  result in streams)
            {
                files.Add(result.Key, result.Value.ToArray());
                result.Value.Close();
            }
            return files;
        }

        public async Task UploadAsync( IFormFile file, string uri)
        {           
           await _storageClient.UploadObjectAsync(_bucketName, uri, null, file.OpenReadStream());
        }

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

    //Registered as a singleton service to avoid creating multiple clients
    public class StorageClientProvider
    {
        private readonly StorageClient _client;

        public StorageClientProvider() {
            var credentialsPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_PATH");
            if (credentialsPath == null)
            {
                throw new Exception("Could not retrieve path for Google Firebase Credentials");
            }
           var  _keyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, credentialsPath);
            var credential = GoogleCredential.FromFile(_keyFilePath);
            _client = StorageClient.Create(credential);

        }

        public StorageClient GetClient()
        {
            return _client;
        }
    }
}
