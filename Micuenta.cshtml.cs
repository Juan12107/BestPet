using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;

namespace BESTPET_DEFINITIVO.Pages
{
    [Authorize]
    public class MiCuentaModel : PageModel
    {
        private readonly AppDbContext _context;

        public MiCuentaModel(AppDbContext context)
        {
            _context = context;
        }

        public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue("Id");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Esta consulta carga al usuario Y su lista de mascotas si es un cliente
            Usuario = await _context.Usuarios
                                    .Include(u => u.Mascotas)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (Usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Page();
        }
    }
}