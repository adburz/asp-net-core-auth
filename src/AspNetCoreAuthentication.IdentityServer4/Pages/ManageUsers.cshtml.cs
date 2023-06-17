using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreAuthentication.IdentityServer4.Pages;

[Authorize("ManageUsers")]
public class ManageUsersModel : PageModel
{
    public void OnGet()
    {
    }
}