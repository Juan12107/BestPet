using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BESTPET_DEFINITIVO.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRutaFotoAUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaFoto",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaFoto",
                table: "Usuarios");
        }
    }
}
