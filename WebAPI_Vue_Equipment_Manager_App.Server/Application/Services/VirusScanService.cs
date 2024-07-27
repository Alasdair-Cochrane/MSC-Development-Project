using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public interface IVirusScanService
    {
        Task<string> ScanFile(IFormFile file);
    }

    public class VirusScanService : IVirusScanService
    {
        private readonly string _api = "https://www.virustotal.com/api/v3/files";
        private readonly IConfiguration _configuration;

        private readonly IHttpClientFactory _httpClientFactory;
        public VirusScanService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        //can use application secrets(accessed via config) for development
        //but it is not recommended for production - use environment variables instead

        public async Task<string> ScanFile(IFormFile file)
        {
            //var client = _httpClientFactory.CreateClient();
            //client.BaseAddress = new Uri(_api);
            //client.DefaultRequestHeaders.Add("x-apikey", _configuration["VIRUS_TOTAL_API_KEY"]);
            //client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");


            ////https://stackoverflow.com/questions/72853828/http-post-multipart-formdata-using-httpclient
            //using var formContent = new MultipartFormDataContent();
            //using var stream = file.OpenReadStream();
            //formContent.Add(new StreamContent(stream),"file");

            //var response = await client.PostAsync(_api, formContent);

            var x = "hello";

            return x;
        }


    }
}
