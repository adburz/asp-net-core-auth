using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AspNetCoreAuthentication.IdentityServer4.Pages;

public record SessionData(IEnumerable<Claim> Claims, IDictionary<string, string> Metadata);

public class SecureModel : PageModel
{
    public SessionData Session { get; set; }

    public async void OnGet()
    {

        var result = await HttpContext.AuthenticateAsync();

        var claims = result.Principal?.Claims;
        var metadata = result.Properties?.Items;

        Session = new SessionData(claims, metadata);
    }
}
