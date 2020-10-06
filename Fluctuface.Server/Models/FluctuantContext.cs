using Microsoft.EntityFrameworkCore;

namespace Haiku.Fluctuface.Server.Models
{
    public class FluctuantContext : DbContext
    {
        public FluctuantContext(DbContextOptions<FluctuantContext> options)
            : base(options)
        {
        }

        public DbSet<FluctuantVariable> FluctuantVariables { get; set; }
    }
}
