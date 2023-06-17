using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreAuthentication.IdentityServer4.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return SignOut(authenticationSchemes: new[] { "cookie", "openid" });
        }
    }
}
