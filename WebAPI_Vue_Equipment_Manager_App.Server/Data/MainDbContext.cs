using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;


namespace WebAPI_Vue_Equipment_Manager_App.Server.Data
{
    public class MainDbContext : DbContext 
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<EquipmentModel> Models { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<UserAssignment> Assignments { get; set; }
        public DbSet<MaintenanceFrequency> MaintenanceFrequencys { get;set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<MaintenanceCategory> MaintenanceTypes { get; set; }
        public DbSet<ItemNote> ItemNotes { get; set; }
        public DbSet<EquipmentModelCategory> EquipmentTypes { get; set;}
        public DbSet<ItemDocument> ItemDocuments { get; set; }
        public DbSet<EquipmentModelDocument> ModelDocuments { get; set; }

        
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        //A protected constructor is required to subclass from a parent inheriting from DBContext
        //https://stackoverflow.com/questions/41829229/how-do-i-implement-dbcontext-inheritance-for-multiple-databases-in-ef7-net-co
        protected MainDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MaintenanceCategory>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<EquipmentModelCategory>().HasIndex(e => e.Name).IsUnique();

            builder.Entity<Unit>().HasData(
                new Unit { Id = -1, Name = "Admin" });

            builder.Entity<UserRole>().HasData(
                new UserRole { Id = -1,Name = "Administrator" });

            builder.Entity<EquipmentModelCategory>().HasData(
                new EquipmentModelCategory { Id = -1,Name = "Centrifuge" });
        }

    }
}
