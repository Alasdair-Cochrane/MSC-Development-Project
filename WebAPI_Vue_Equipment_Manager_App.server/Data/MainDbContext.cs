using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;


namespace WebAPI_Vue_Equipment_Manager_App.Server.Data
{
    //Used by EF Core to construct required table sin the database
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

            //setting up one-to-many foreign keys to avoid having navigation properties in child entity
            builder.Entity<Item>().
                 HasMany(x => x.Maintenances).
                 WithOne().
                 HasForeignKey(i => i.ItemId);

           
            builder.Entity<Item>().
                HasMany(i => i.Documents).
                WithOne().
                HasForeignKey(d => d.ItemId);

            builder.Entity<Maintenance>().
                HasMany(m => m.Documents).
                WithOne().
                HasForeignKey(d => d.MaintenanceId);

            builder.Entity<Item>().
               HasMany(i => i.Notes).
               WithOne().
               HasForeignKey(d => d.ItemId);

            builder.Entity<User>().
                HasMany<ItemNote>().
                WithOne().
                HasForeignKey(d => d.UserId);
            

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
            builder.Entity<ItemStatusCategory>().HasData(
               new ItemStatusCategory { Id = 1, Name = "Active", Order = 1 , ColorHex = "#66BB6A" },
               new ItemStatusCategory { Id = 2, Name = "In Storage", Order = 2 , ColorHex = "#4FC3F7" },
               new ItemStatusCategory { Id = 3, Name = "Broken", Order = 3 , ColorHex = "#EF5350" },
               new ItemStatusCategory { Id = 4, Name = "Decommissioned", Order = 4 , ColorHex = "#BDBDBD" },
               new ItemStatusCategory { Id = 5, Name = "On Loan", Order = 5 , ColorHex = "#BA68C8" },
               new ItemStatusCategory { Id = 6, Name = "Requires Installation", Order = 6 , ColorHex = "#FFD54F" },
               new ItemStatusCategory { Id = 7, Name = "Requires Service", Order = 7 , ColorHex = "#FFB300" },
               new ItemStatusCategory { Id = 8, Name = "Requires Calibration", Order = 8 , ColorHex = "#FF8F00" },
               new ItemStatusCategory { Id = 9, Name = "Requires Validation", Order = 9 , ColorHex = "#FFCA28" }
               );

            builder.Entity<MaintenanceCategory>().HasData(
                new MaintenanceCategory { Id = 1, Name = "Repair" },
                new MaintenanceCategory { Id = 2, Name = "Calibration" },
                new MaintenanceCategory { Id = 3, Name = "Service" },
                new MaintenanceCategory { Id = 4, Name = "Validation" },
                new MaintenanceCategory { Id = 5, Name = "Decommissioning" },
                new MaintenanceCategory { Id = 6, Name = "Decontamination" }
                );



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
