namespace Mango.Services.Identity.Services;

using System.Security.Claims;
using System.Threading.Tasks;

using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using IdentityModel;

using Mango.Services.Identity.Models;

using Microsoft.AspNetCore.Identity;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public ProfileService(
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string subjectId = context.Subject.GetSubjectId();
        ApplicationUser user = await this.userManager.FindByIdAsync(subjectId);
        ClaimsPrincipal userClaims = await this.userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.FirstName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.LastName));

        if (this.userManager.SupportsUserRole)
        {
            IList<string> roles = await this.userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (this.roleManager.SupportsRoleClaims)
                {
                    IdentityRole role = await this.roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        claims.AddRange(await this.roleManager.GetClaimsAsync(role));
                    }
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string subjectId = context.Subject.GetSubjectId();
        ApplicationUser user = await this.userManager.FindByIdAsync(subjectId);
        context.IsActive = user != null;
    }
}
