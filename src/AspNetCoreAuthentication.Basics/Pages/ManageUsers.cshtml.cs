using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenIdSample.Web.Pages;

[Authorize("ManageUsers")]
public class ManageUsersModel : PageModel
{
    public void OnGet()
    {
    }
}
