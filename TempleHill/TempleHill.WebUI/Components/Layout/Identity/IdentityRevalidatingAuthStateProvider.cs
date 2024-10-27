using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TempleHill.Application.Extensions.Identity;

namespace TempleHill.WebUI.Components.Layout.Identity
{
    //please do ensure to register this service
    //for authenticating the user  check if the user exist (Identity Re-validating Authentication State Provider frequently)
    public sealed class IdentityRevalidatingAuthStateProvider(ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,IOptions<IdentityOptions> options)
        : RevalidatingServerAuthenticationStateProvider(loggerFactory)
    {

        //for re-validating the login user as soon as the connection is still open for this below timespan
        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(20);



        //continue to re validate authentication state of the login user
        protected override async Task<bool> ValidateAuthenticationStateAsync
            (AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }


        //continue to re validate security stamp  of the login user

        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager,
            ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if(user == null)
            {
                return false;
            }
            else if (userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);
                var userstamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userstamp;

            }
        }
    }
}
