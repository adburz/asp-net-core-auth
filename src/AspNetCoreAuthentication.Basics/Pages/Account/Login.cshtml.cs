using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace OpenIdSample.Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                var user = HttpContext.User;
                var claims = new List<Claim>
                {
                    new Claim(type: "sub", value: Guid.NewGuid().ToString()),
                    new Claim(type: "name", value: "Manat"),
                    new Claim(type: "role", value: "role"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims: claims,
                    authenticationType: "pwd",
                    nameType: "name",
                    roleType: "role");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                return LocalRedirect(ReturnUrl);
            }

            return Page();
        }


    }
}
