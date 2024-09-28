using GHSTShipping.Infrastructure.Identity.Enums;
using GHSTShipping.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            // Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "mjsshunnjer@gmail.com",
                Name = "Tu Van",
                PhoneNumber = "0974255412",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Type = AccountTypeConstants.ADMIN
            };

            if (!await userManager.Users.AnyAsync())
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Tu@2024");
                }
            }
        }
    }
}
