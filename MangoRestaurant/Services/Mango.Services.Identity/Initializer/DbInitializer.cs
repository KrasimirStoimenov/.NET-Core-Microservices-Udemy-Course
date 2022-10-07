namespace Mango.Services.Identity.Initializer;

using System.Security.Claims;

using IdentityModel;

using Mango.Services.Identity.DbContext;
using Mango.Services.Identity.Models;

using Microsoft.AspNetCore.Identity;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext dbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public DbInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task Initialize()
    {
        if (this.roleManager.FindByNameAsync(StaticDetails.Admin).Result == null)
        {
            await this.roleManager.CreateAsync(new IdentityRole(StaticDetails.Admin));
            await this.roleManager.CreateAsync(new IdentityRole(StaticDetails.Customer));
        }
        else
        {
            return;
        }

        await SeedAdmin();
        await SeedCustomer();
    }

    private async Task SeedAdmin()
    {
        ApplicationUser adminUser = new ApplicationUser()
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0123456789",
            FirstName = "Some",
            LastName = "Admin"
        };

        await this.userManager.CreateAsync(adminUser, "Admin123$");
        await this.userManager.AddToRoleAsync(adminUser, StaticDetails.Admin);

        IdentityResult adminClaims = this.userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName),
            new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
            new Claim(JwtClaimTypes.Role,StaticDetails.Admin),
        }).Result;
    }

    private async Task SeedCustomer()
    {
        ApplicationUser customerUser = new ApplicationUser()
        {
            UserName = "customer",
            Email = "customer@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0123456789",
            FirstName = "Some",
            LastName = "Customer"
        };

        await this.userManager.CreateAsync(customerUser, "Customer123$");
        await this.userManager.AddToRoleAsync(customerUser, StaticDetails.Customer);

        IdentityResult customerClaims = this.userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
            new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
            new Claim(JwtClaimTypes.Role,StaticDetails.Customer),
        }).Result;
    }

}
