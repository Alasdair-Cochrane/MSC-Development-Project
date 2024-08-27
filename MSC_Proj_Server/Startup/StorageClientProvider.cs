using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Startup
{
    //Registered as a singleton service to avoid creating multiple clients
    public class StorageClientProvider
    {
        private readonly StorageClient _client;

        public StorageClientProvider()
        {
            var credentialsPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_PATH");
            if (credentialsPath == null)
            {
                throw new Exception("Could not retrieve path for Google Firebase Credentials");
            }
            var _keyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, credentialsPath);
            var credential = GoogleCredential.FromFile(_keyFilePath);
            _client = StorageClient.Create(credential);

        }

        public StorageClient GetClient()
        {
            return _client;
        }
    }
}
