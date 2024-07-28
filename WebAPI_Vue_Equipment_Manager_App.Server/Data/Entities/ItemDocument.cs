using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class ItemDocument : IDocumentRelation
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; } = null!;
    }
}
