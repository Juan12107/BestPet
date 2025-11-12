using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BESTPET_DEFINITIVO.Data;
using BESTPET_DEFINITIVO.Models;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.IO;

namespace BESTPET_DEFINITIVO.Pages.Usuarios
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuario { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Usuario = await _context.Usuarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Usuarios");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "ID";
                worksheet.Cell(currentRow, 2).Value = "Nombre Completo";
                worksheet.Cell(currentRow, 3).Value = "Correo";
                worksheet.Cell(currentRow, 4).Value = "Rol";
                worksheet.Cell(currentRow, 5).Value = "Fecha de Registro";

                foreach (var user in usuarios)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.Id;
                    worksheet.Cell(currentRow, 2).Value = user.Nombre + " " + user.Apellido;
                    worksheet.Cell(currentRow, 3).Value = user.Correo;
                    worksheet.Cell(currentRow, 4).Value = user.Rol;
                    worksheet.Cell(currentRow, 5).Value = user.FechaRegistro;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteDeUsuarios.xlsx");
                }
            }
        }
    }
}