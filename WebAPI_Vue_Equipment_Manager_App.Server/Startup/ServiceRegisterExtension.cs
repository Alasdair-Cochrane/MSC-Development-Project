using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Startup
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(ICategoryRepository<>), typeof(GenericCategoryRepository<>));

            services.AddScoped<DocumentRepository>();

            services.AddScoped<IEquipmentModelRepository, EquipmentModelRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
            services.AddScoped<IItemQueryBuilder, ItemQueryBuilder>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            //entity services
            services.AddScoped<IEquipmentModelService, EquipmentModelsService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<INotesService, NotesService>();

            //Storage services
            services.AddSingleton<StorageClientProvider>();
            services.AddScoped<IDocumentService, FirebaseDocumentService>();
            services.AddScoped<IImageService, FirebaseImageService>();
                        
            //caches for database tables with small number of rows
            services.AddSingleton<IEntityCache<EquipmentModelCategory>, InMemoryEntityCache<EquipmentModelCategory>>();
            services.AddSingleton<IEntityCache<MaintenanceCategory>, InMemoryEntityCache<MaintenanceCategory>>();
            services.AddSingleton<IEntityCache<ItemStatusCategory>, InMemoryEntityCache<ItemStatusCategory>>();

            //Ai service
            services.AddScoped<ILabelInterpretationService, GoogleLabelInterpretationService>();

        }
    }
}
