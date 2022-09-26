using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SocialMediaMustBeGood2.Models
{
    public class AdminInitializer
    {
        public static async Task SeedAdminUser(
              RoleManager<IdentityRole<int>> _roleManager,
              UserManager<User> _userManager)
        {
            string adminName = "admin";
            string adminPassword = "pass";

            var roles = new[] { "admin", "user" };

            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) is null)
                    await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
            if (await _userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { UserName = adminName, Email = "admin@admin.ad" };
                IdentityResult result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }

}
