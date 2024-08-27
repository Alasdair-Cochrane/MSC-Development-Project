namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes
{
    public abstract class Category : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
