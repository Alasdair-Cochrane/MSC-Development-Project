using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class ItemStatusCategory : Category
    {
        public ICollection<Item> Members { get; set; } = null!;

        public int Order {  get; set; }
    }
}
