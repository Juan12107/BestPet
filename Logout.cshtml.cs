using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace BESTPET_DEFINITIVO.Pages
{
    public class LogoutModel : PageModel
    {
        // Este método se puede llamar al cargar la página o al enviar un formulario
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToPage("/Index");
        }
    }
}