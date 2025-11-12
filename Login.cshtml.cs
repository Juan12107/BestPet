using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress]
            public string Correo { get; set; } // Debe llamarse Correo

            [Required(ErrorMessage = "La contraseña es obligatoria.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } // Debe llamarse Password
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == Input.Correo);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Input.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña inválidos.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            if (user.Rol == "Admin")
            {
                return RedirectToPage("/Usuarios/Index");
            }
            else
            {
                return RedirectToPage("/MiCuenta");
            }
        }
    }
}