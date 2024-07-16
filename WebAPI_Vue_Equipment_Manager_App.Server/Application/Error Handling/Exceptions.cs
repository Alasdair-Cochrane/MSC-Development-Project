using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling
{
    //https://stackoverflow.com/questions/2200241/in-c-sharp-how-do-i-define-my-own-exceptions

    [Serializable]
    public class DataInsertionException : Exception
    {
        public readonly string Entity;
       
        public DataInsertionException(string message, string entity = "") : base(message)
        {
            Entity = entity;
        }
        public DataInsertionException(string message, Exception innerException, string entity = "") : base(message, innerException)
        {
            Entity = entity;
        }

    }
    public class ImageUploadException : Exception {
    
        public ImageUploadException(string message) : base(message) { }
        public ImageUploadException(string message, Exception innerException) :base(message,innerException) { }
    }

    public class UnitException : Exception { 
    
        public UnitException(string message) : base(message) { }
        public UnitException(string message, Exception innerException):  base(message, innerException) { }
    
    }

}
