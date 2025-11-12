using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages.Usuarios
{
    public class DetailsModel : PageModel
    {
        private readonly BESTPET_DEFINITIVO.Data.AppDbContext _context;

        public DetailsModel(BESTPET_DEFINITIVO.Data.AppDbContext context)
        {
            _context = context;
        }

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                Usuario = usuario;
            }
            return Page();
        }
    }
}