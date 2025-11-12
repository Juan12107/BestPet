using System.ComponentModel.DataAnnotations.Schema;
namespace BESTPET_DEFINITIVO.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Genero { get; set; } // Reemplaza a Especie
        public string Raza { get; set; }
        public int Edad { get; set; }
        public string? RutaFotoMascota { get; set; }

        // --- NUEVOS CAMPOS ---
        public string? Vacunas { get; set; }
        public string? Recomendaciones { get; set; }

        // Clave foránea para relacionar la mascota con su dueño
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Dueño { get; set; }
    }
}