using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;

namespace Presistence.Data.Seeding
{
    public class DbInitializer(
        AppDbContext _context,
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole> _roleManager
        ) : IDbInitializer
    {
        public async Task IdentityInitializeAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }

            if (!_context.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                   Name = "SuperAdmin",   
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                   Name = "Admin",
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                   Name = "Instructor",
                });
            }

            if (!_context.Users.Any())
            {
                var SuperAdmin = new AppUser
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01233345555"
                };
                var Admin = new AppUser
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01233345555"
                };

                await _userManager.CreateAsync(SuperAdmin, Environment.GetEnvironmentVariable("AdminPassword")!);
                await _userManager.CreateAsync(Admin, Environment.GetEnvironmentVariable("AdminPassword")!);


                await _userManager.AddToRoleAsync(SuperAdmin , "SuperAdmin");
                await _userManager.AddToRoleAsync(Admin , "Admin");
            };
        }
    }
}