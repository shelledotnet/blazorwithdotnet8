using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TempleHill.Application.Extensions.Identity;

namespace TempleHill.WebUI.Components.Layout.Identity
{
    internal static class SignOutEndPoint
    {

        //add this to the middle wear => app.MapSignOutEndPoint();
        public static IEndpointConventionBuilder MapSignOutEndPoint(this IEndpointRouteBuilder endpoint)
        {
            ArgumentNullException.ThrowIfNull(endpoint);
            var accountGroup = endpoint.MapGroup("/Account");
            accountGroup.MapPost("/Logout",async(ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager)=>
            {
                //this will clear the token from the cookies storage(if u loose the cookies u loose the session)
                await signInManager.SignOutAsync();

                //Landing component after sign out
                return TypedResults.LocalRedirect("/Account/Login");
            });

            return accountGroup;

        }
    }
}
