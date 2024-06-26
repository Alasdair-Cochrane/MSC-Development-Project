using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class MaintenanceMappingExtensions
    {
        public static MaintenanceDTO ToDTO(this Maintenance entity) {

            var dto = new MaintenanceDTO
            {
                Id = entity.Id,
                ItemId = entity.ItemId,
                Provider_Name = entity.Provider_Name,
                Date_Completed = entity.Date_Completed,
                CategoryName = entity.Category.Name
            };
            return dto;  
        }

        public static Maintenance ToEntity(this MaintenanceDTO dto, int CategoryId) 
        {
            var entity = new Maintenance
            {
                Id = dto.Id,
                ItemId = dto.ItemId,
                Provider_Name = dto.Provider_Name,
                Date_Completed = dto.Date_Completed,
                CategoryId = CategoryId,
            };
            return entity;    
        }
    }

}
