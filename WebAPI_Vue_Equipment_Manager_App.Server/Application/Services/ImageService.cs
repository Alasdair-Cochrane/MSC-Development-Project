using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class ServerImageService : IImageService
    {
        private readonly string _baseDirectory;
        public ServerImageService(IWebHostEnvironment environment) {

            _baseDirectory = Path.Combine(environment.WebRootPath, "images");
        
        }

       public async Task<byte[]?> Retrieve(string fileName)
        {
            try
            {
                var image = await File.ReadAllBytesAsync(Path.Combine(_baseDirectory, fileName));
                return image;
            }
            catch {
                //should replace with specific execptions - filenot found exception
                return null;
            }
           
            
            
        }

//        https://www.youtube.com/watch?v=noXM1UsTAyo
        public async Task Upload(IFormFile file, string fileName)
        {  
            
            using(var stream = File.Create(Path.Combine(_baseDirectory, fileName)))
            {
                
                await file.CopyToAsync(stream);
            }
            
        }
       
    }

    public interface IImageService {

        public Task<byte[]?> Retrieve(string fileName);
        public Task Upload(IFormFile file, string fileName);

    }

}
