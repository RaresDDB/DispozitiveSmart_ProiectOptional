using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineEnergyUtilityPlatform.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineEnergyUtilityPlatform.Data
{
    public class EnergyPlatformDbContext: IdentityDbContext<User, Role, string>
    {
        public EnergyPlatformDbContext(DbContextOptions<EnergyPlatformDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            const string ADMIN_ID = "FC977605-042A-4A07-ABBE-F92F13B930DA";
            const string ADMIN_ROLE_ID = "CC79362D-A99D-400C-98B6-C7A1565FF408";
            const string USER_ROLE_ID = "2292CE1E-1249-4E27-9156-C185F8C53081";

            modelBuilder.Entity<Role>().HasData(
                new Role {
                    Id = ADMIN_ROLE_ID,
                    Name = "admin",
                    NormalizedName = "ADMIN",
                },
                new Role { 
                    Id = USER_ROLE_ID,
                    Name = "client",
                    NormalizedName = "CLIENT",
                }
            );

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(u =>
                new
                {
                    u.UserId,
                    u.RoleId
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                }
             );

            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(u =>
                new
                {
                    u.UserId,
                    u.LoginProvider,
                    u.Name
                }
            );

            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(u =>
                new
                {
                    u.Id
                }
            );

            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(u =>
                new
                {
                    u.Id
                }
            );

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(u =>
                new
                {
                    u.LoginProvider,
                    u.ProviderKey,
                    u.UserId
                }
            );

            var adminUser = new User
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                SecurityStamp = string.Empty
            };

            modelBuilder.Entity<User>().HasData(
                adminUser
            );

        }

        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<UserRole> UserRole { get; set; }   
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<UserClaim> UserClaim { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
    }
}
