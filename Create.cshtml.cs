using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims; // Necesario para la sesión
using Microsoft.AspNetCore.Authentication; // Necesario para la sesión

namespace BESTPET_DEFINITIVO.Pages.Usuarios
{
    public class CreateModel : PageModel
    {
        private readonly BESTPET_DEFINITIVO.Data.AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(BESTPET_DEFINITIVO.Data.AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        [BindProperty]
        public IFormFile? FotoPerfil { get; set; }

        [BindProperty]
        public IFormFile? ArchivoExperiencia { get; set; }

        [BindProperty, DataType(DataType.Password), Required(ErrorMessage = "La contraseña es obligatoria.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [BindProperty, DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);

            if (FotoPerfil != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FotoPerfil.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes/usuarios");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await FotoPerfil.CopyToAsync(fileStream); }
                Usuario.RutaFoto = "/imagenes/usuarios/" + uniqueFileName;
            }

            if (Usuario.Rol == "Paseador" && ArchivoExperiencia != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArchivoExperiencia.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "documentos/experiencia");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await ArchivoExperiencia.CopyToAsync(fileStream); }
                Usuario.RutaArchivoExperiencia = "/documentos/experiencia/" + uniqueFileName;
            }

            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();

            // --- ¡NUEVA LÓGICA DE INICIO DE SESIÓN AUTOMÁTICO! ---
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Usuario.Nombre),
                new Claim(ClaimTypes.Email, Usuario.Correo),
                new Claim("Id", Usuario.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            // --- ¡Y LA NUEVA REDIRECCIÓN! ---
            return RedirectToPage("/MiCuenta");
        }
    }
}