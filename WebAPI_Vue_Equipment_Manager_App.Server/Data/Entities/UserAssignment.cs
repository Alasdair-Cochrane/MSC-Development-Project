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

        public User User { get; set; }
        public Unit Unit { get; set; }
        public UserRole Role { get; set; }

    }
}
