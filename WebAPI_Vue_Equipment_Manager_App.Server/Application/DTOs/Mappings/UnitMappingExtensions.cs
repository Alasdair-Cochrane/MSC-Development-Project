using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class UnitMappingExtensions
    {
        public static UnitDTO ToDTO (this Unit unit, IEnumerable<AssignmentDTO>? assignments = null)
        {
            var dto = new UnitDTO
            {
                Id = unit.Id,
                Name = unit.Name,
                Building = unit.Building,
                Room = unit.Room,
                Address = unit.Address,
                ParentId = unit.ParentId,
                AssigedUsers = assignments?.ToList()
            };
            return dto;
        }

        public static Unit ToEntity(this UnitDTO unit)
        {
            var entity = new Unit
            {
                Id = unit.Id ?? 0,
                Name = unit.Name,
                Building = unit.Building,
                Room = unit.Room,
                Address = unit.Address,
                ParentId = unit.ParentId,
            };
            return entity;
        }
    }
}
