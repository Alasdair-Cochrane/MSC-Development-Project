using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class Document : IEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        
        public required string URL { get; set; }

        public required string FileName { get; set; }

    }
}
