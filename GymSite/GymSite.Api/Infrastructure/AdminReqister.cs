using GymSite.Database;
using GymSite.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GymSite.Api.Infrastructure
{
    public static class AdminReqister
    {
        public static void RegisterAdmin(this WebApplication app)
        {
            try
            {
                // Creating admin user if one does not exist
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    context.Database.EnsureCreated();

                    var createAdmin = !context.UserClaims.Any(x => x.ClaimValue == "Admin");

                    if (createAdmin)
                    {
                        Console.WriteLine("Admin user does not exist!");
                        Console.WriteLine("Do you want to create it? y/n");
                        var response = Console.ReadLine().ToLower();
                        createAdmin = response == "y" || response == "yes";
                    }

                    if (createAdmin)
                    {
                        Console.WriteLine();
                        Console.Write("Username: ");
                        var username = Console.ReadLine();
                        Console.Write("Nickname: ");
                        var nick = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        var admin = new ApplicationUser
                        {
                            UserName = username,
                            NickName = nick
                        };

                        userManager.CreateAsync(admin, password).GetAwaiter().GetResult();

                        var adminClaim = new Claim("Role", "Admin");

                        userManager.AddClaimAsync(admin, adminClaim).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
