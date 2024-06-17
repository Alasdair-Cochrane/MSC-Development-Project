using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class EquipmentModelDocument : IEntity
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public required EquipmentModel Model { get; set; }
        [MaxLength(100)]
        public required string URL { get; set; }

    }
}
