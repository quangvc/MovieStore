using Microsoft.AspNetCore.Identity;
using MovieStoreMvc.Constants;
using MovieStoreMvc.Models;

namespace MovieStoreMvc.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            
            //Seed Roles
            var userManager = service.GetService<UserManager<AppUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // creating admin

            var user = new AppUser
            {
                UserName = "Admin@gmail.com",
                Email = "Admin@gmail.com",
                Name = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
