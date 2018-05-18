using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace User.API.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.AppUser>().ToTable("Users").HasKey(u => u.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Models.AppUser> Users { get; set; }
    }
}