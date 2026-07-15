using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniBlog.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToPage("/Posts/Index");
        }
    }
}