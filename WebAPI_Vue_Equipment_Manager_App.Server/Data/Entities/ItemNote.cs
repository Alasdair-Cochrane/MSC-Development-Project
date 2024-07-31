using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class ItemNote : IEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }

        [MaxLength(1000)]
        public required string Text { get; set; }
        [MaxLength(100)]
        public required string Title { get; set; }
    }
}
