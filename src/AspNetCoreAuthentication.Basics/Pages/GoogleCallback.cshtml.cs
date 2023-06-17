using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace OpenIdSample.Web.Pages
{
    [AllowAnonymous]
    public class GoogleCallbackModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            //Read the outcome of the external authentication
            var result = await HttpContext.AuthenticateAsync("cookie-google-tmp");
            if (!result.Succeeded)
                throw new Exception("External authentication failed.");

            //logic
            var externalUser = result.Principal;
            var sub = externalUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var name = externalUser.FindFirst(ClaimTypes.Name).Value;
            var email = externalUser.FindFirst(ClaimTypes.Email).Value;
            var issuer = result.Properties.Items["scheme"];

            // sign-in the user in our app

            var claims = new List<Claim>
                {
                    new Claim(type: "sub", value: sub),
                    new Claim(type: "name", value: name),
                    new Claim(type: "email", value: email),
                    //new Claim(type: "role", value: "role"), -> google doesnt know anything about "role" claim.
                };

            var claimsIdentity = new ClaimsIdentity(claims: claims,
                authenticationType: issuer,
                nameType: "name",
                roleType: "role");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);
            await HttpContext.SignOutAsync("cookie-google-tmp");

            var returnUrl = result.Properties.Items["uru"];
            return Redirect(returnUrl);
        }
    }
}
