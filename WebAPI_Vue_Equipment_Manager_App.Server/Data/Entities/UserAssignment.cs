using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    [PrimaryKey(nameof(UserId), nameof(UnitId))]
    public class UserAssignment
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int UnitId { get; set; }
        public int RoleId { get; set; }

    }
}
