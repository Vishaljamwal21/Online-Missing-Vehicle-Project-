using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using onlinemissingvehical.Models;

namespace onlinemissingvehical.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MissingVehicle> MissingVehicles { get; set; }
        public DbSet<StatusUpdate> StatusUpdates { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
