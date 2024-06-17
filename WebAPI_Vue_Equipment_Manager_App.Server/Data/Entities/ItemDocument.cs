using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class ItemDocument : IEntity
    {
        public required int Id { get; set; }
        public required int ItemId { get; set; }
        [MaxLength(100)]

        public required string URL { get; set; }

    }
}
