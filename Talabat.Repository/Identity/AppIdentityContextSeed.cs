using Microsoft.AspNetCore.Identity;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "test test",
                    Email = "test@test.com",
                    UserName = "test123",
                    PhoneNumber = "01234567891"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
