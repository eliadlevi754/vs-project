using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace vs_project.Pages
{
    public class verifyAdminModel : PageModel
    {
        public void OnGet()
        {
            TempData.Keep("isAdmin");
            TempData.Keep("username");
            TempData.Keep("password");
            TempData.Keep("Pname");
            TempData.Keep("Fname");
            
        }

    }
}
