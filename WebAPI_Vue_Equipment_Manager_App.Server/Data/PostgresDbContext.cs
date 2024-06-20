using Microsoft.EntityFrameworkCore;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data
{
    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }
    }
}
