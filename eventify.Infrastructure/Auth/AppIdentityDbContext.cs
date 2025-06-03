using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace eventify.Infrastructure.Persistence
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Optional: customize identity table names or add seed data here
            builder.Entity<AppUser>(b =>
            {
                b.ToTable("AppUsers");
            });

            builder.Entity<IdentityRole<Guid>>(b =>
            {
                b.ToTable("AppRoles");
            });

            builder.Entity<IdentityUserRole<Guid>>(b =>
            {
                b.ToTable("AppUserRoles");
            });

            builder.Entity<IdentityUserClaim<Guid>>(b =>
            {
                b.ToTable("AppUserClaims");
            });

            builder.Entity<IdentityUserLogin<Guid>>(b =>
            {
                b.ToTable("AppUserLogins");
            });

            builder.Entity<IdentityRoleClaim<Guid>>(b =>
            {
                b.ToTable("AppRoleClaims");
            });

            builder.Entity<IdentityUserToken<Guid>>(b =>
            {
                b.ToTable("AppUserTokens");
            });
        }
    }
}
