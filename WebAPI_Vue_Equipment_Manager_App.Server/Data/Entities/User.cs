using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public ICollection<UserAssignment>? Assignments { get; set; }
    }
}
