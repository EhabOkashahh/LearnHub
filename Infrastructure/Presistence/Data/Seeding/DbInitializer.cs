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
                await _roleManager.CreateAsync(new IdentityRole
                {
                   Name = "Student",
                });
            }
            else
            {
                string[] requiredRoles = ["SuperAdmin", "Admin", "Instructor", "Student"];
                foreach (var roleName in requiredRoles)
                    if (!await _roleManager.RoleExistsAsync(roleName))
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (!await _context.Users.AnyAsync())
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
            }
            else
            {
                var superAdminUser = await _userManager.FindByEmailAsync("SuperAdmin@gmail.com");
                if (superAdminUser is not null && !await _userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

                var adminUser = await _userManager.FindByEmailAsync("Admin@gmail.com");
                if (adminUser is not null && !await _userManager.IsInRoleAsync(adminUser, "Admin"))
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}