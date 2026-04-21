using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task UseUserSeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await CreateRoles(roleManager);
            await CreateUser(userManager);
        }

        private static async Task CreateRoles(RoleManager<AppRole> roleManager)
        {
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                var roleName = role.GetRole();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole
                    {
                        Name = roleName
                    });
                }
            }
        }

        private static async Task CreateUser(UserManager<AppUser> userManager)
        {
            var adminEmail = "admin@gmail.com";

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail
                };

                await userManager.CreateAsync(admin, "Admin123!@#");

                await userManager.AddToRoleAsync(admin, Roles.Admin.GetRole());
            }
        }
    }
}
