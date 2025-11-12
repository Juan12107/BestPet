

namespace BESTPET_DEFINITIVO.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Celular { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? RutaFoto { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Campo para diferenciar roles
        public string Rol { get; set; } = "Cliente";

        // Campos específicos del Paseador
        public string? Descripcion { get; set; }
        public string? RutaArchivoExperiencia { get; set; }

        // Campo para la contraseña encriptada
        public string PasswordHash { get; set; } = string.Empty;

        // Propiedad de navegación para la lista de mascotas
        public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}