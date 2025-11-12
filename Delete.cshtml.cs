using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages.Mascotas
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mascota Mascota { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (mascota == null) return NotFound();

            Mascota = mascota;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                Mascota = mascota;
                _context.Mascotas.Remove(Mascota);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/MiCuenta");
        }
    }
}