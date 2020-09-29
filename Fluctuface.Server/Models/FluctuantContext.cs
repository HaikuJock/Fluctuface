using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fluctuface.Server.Models
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
