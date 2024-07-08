using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision;
using Google.Cloud.Vision.V1;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text.Json;


namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class GoogleLabelRecognitionService : ILabelRecognitionService
    {
        private readonly IWebHostEnvironment _env;

        public GoogleLabelRecognitionService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public async Task<string> ReadImageAsync(IFormFile image)
        {
            Byte[] bytes;

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                bytes = stream.ToArray();
                if (bytes == null || bytes?.Length == 0) { throw new Exception("image was empty"); }
            }

            var apiImage = Image.FromBytes(bytes);
           
            string keyPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")!;
            if (keyPath == null) { throw new Exception("google authentication environment variable not found"); }
            var credentials = GoogleCredential.FromFile(keyPath);
            
            var builder = new ImageAnnotatorClientBuilder();
            builder.GoogleCredential = credentials;
            builder.Endpoint = "eu-vision.googleapis.com";
            var client = builder.Build();

            var response = await client.DetectTextAsync(apiImage);
            if(response.IsNullOrEmpty()) { return "nothing to see"; }
            List<string> descriptions = new List<string>();

            foreach (EntityAnnotation annotation in response)
            {
             
                    descriptions.Add(annotation.Description);
                
            }
            return JsonSerializer.Serialize(descriptions);
        }

    }
}
public interface ILabelRecognitionService {

    public Task<string> ReadImageAsync(IFormFile image);

}

