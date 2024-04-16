using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EhodBoutiqueEnLigne.Data
{
    public static class IdentitySeedData
    {

        public static async Task EnsurePopulated(IApplicationBuilder app, IConfiguration config)
        {
            string AdminUser = config["Administrator:User"];
            string AdminPassword = config["Administrator:Password"];

            using var scope = app.ApplicationServices.CreateScope();
            var userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetService(typeof(UserManager<IdentityUser>));

            IdentityUser user = await userManager.FindByNameAsync(AdminUser);
            if (user == null)
            {
                user = new IdentityUser(AdminUser);
                await userManager.CreateAsync(user, AdminPassword);
            }
        }
    }
}