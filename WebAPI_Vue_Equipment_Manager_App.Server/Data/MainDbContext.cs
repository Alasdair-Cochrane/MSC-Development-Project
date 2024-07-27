using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;


namespace WebAPI_Vue_Equipment_Manager_App.Server.Data
{
    public class MainDbContext : IdentityDbContext<User,UserRole,int>
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<EquipmentModel> Models { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserAssignment> Assignments { get; set; }
        public DbSet<MaintenanceFrequency> MaintenanceFrequencys { get;set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<MaintenanceCategory> MaintenanceTypes { get; set; }
        public DbSet<ItemNote> ItemNotes { get; set; }
        public DbSet<EquipmentModelCategory> EquipmentTypes { get; set;}
        public DbSet<Document> Documents { get; set; }
        public DbSet<EquipmentModelDocument> ModelDocuments { get; set; }
        public DbSet<ItemDocument> ItemDocuments { get; set; }
        public DbSet<MaintenanceDocument> MaintenanceDocuments { get; set; }

        
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        //A protected constructor is required to subclass from a parent inheriting from DBContext
        //https://stackoverflow.com/questions/41829229/how-do-i-implement-dbcontext-inheritance-for-multiple-databases-in-ef7-net-co
        protected MainDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MaintenanceCategory>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<EquipmentModelCategory>().HasIndex(e => e.Name).IsUnique();
            builder.Entity<ItemStatusCategory>().HasIndex(e => e.Name).IsUnique();


            builder.Entity<Unit>().
                HasOne<Unit>().
                WithMany(x => x.Children).
                HasForeignKey(x => x.ParentId);

            builder.Entity<Unit>().
                Property(x => x.IsPublic).
                HasDefaultValue(false);
          

            builder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, Name = "Administrator", NormalizedName = "ADMIN" },
                new UserRole { Id = 2, Name = "Private User", NormalizedName = "PRIVATE" },
                new UserRole { Id = 3, Name = "Public User", NormalizedName = "PUBLIC" }
                );
            //builder.Entity<EquipmentModelCategory>().HasData(GetSeedCategories());

           
        }

        private EquipmentModelCategory[] GetSeedCategories()
        {
            //From CHATGPT
            var  labEquipmentDetailed = SeedData.EquipmentModelCategories;
            int id = 1;
            var categories = labEquipmentDetailed.Select(x => new EquipmentModelCategory
            {
                Name = x,
                Id = id++,
            });
            return categories.ToArray();
        }

    }
}
