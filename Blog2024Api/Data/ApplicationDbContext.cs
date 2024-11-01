using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog2024Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog2024Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int, IdentityUserClaim<int>,
                                                         UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, 
                                                         IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

      
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
