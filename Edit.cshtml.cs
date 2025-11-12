using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages.Mascotas
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Mascota Mascota { get; set; } = default!;

        [BindProperty]
        public IFormFile? FotoMascota { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (mascota == null) return NotFound();

            Mascota = mascota;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (FotoMascota != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FotoMascota.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes/mascotas");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await FotoMascota.CopyToAsync(fileStream); }
                Mascota.RutaFotoMascota = "/imagenes/mascotas/" + uniqueFileName;
            }

            _context.Attach(Mascota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Mascotas.Any(e => e.Id == Mascota.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/MiCuenta");
        }
    }
}