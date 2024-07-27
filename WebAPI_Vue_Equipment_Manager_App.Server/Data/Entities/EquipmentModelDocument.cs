using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class EquipmentModelDocument : IDocumentRelation
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }

        public int ModelId { get; set; }
        [MaxLength(100)]

        public Document Document { get; set; } = null!;
        public EquipmentModel Model { get; set; } = null!;


    }
}
