using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages.Mascotas
{
    [Authorize] // Protegemos la página, solo usuarios logueados pueden agregar mascotas
    public class AgregarMascotaModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AgregarMascotaModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Mascota Mascota { get; set; }

        [BindProperty]
        public IFormFile? FotoMascota { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Obtenemos el ID del dueño (el usuario que ha iniciado sesión)
            var dueñoId = User.FindFirstValue("Id");
            Mascota.UsuarioId = int.Parse(dueñoId);

            // Guardamos la foto de la mascota
            if (FotoMascota != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FotoMascota.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes/mascotas");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                Directory.CreateDirectory(uploadsFolder);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await FotoMascota.CopyToAsync(fileStream); }
                Mascota.RutaFotoMascota = "/imagenes/mascotas/" + uniqueFileName;
            }

            _context.Mascotas.Add(Mascota);
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = $"¡Has registrado a {Mascota.Nombre} con éxito!";
            return RedirectToPage("/MiCuenta");
        }
    }
}