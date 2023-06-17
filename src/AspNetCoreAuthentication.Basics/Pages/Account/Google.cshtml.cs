using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenIdSample.Web.Pages.Account;

[AllowAnonymous]
public class GoogleModel : PageModel
{
    public IActionResult OnGet(string returnUrl)
    {
        //await HttpContext.ChallengeAsync(scheme: "google");
        if (!Url.IsLocalUrl(returnUrl))
        {
            throw new ArgumentException("Invalid Return URL");
        }

        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/GoogleCallback"),
            Items =
            {
                {"uru", returnUrl },
                {"scheme", "google" }
            }
        };

        return Challenge(properties: props, authenticationSchemes: "google");
    }
}
