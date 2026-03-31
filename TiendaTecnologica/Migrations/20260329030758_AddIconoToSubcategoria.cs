using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaTecnologica.Migrations
{
    /// <inheritdoc />
    public partial class AddIconoToSubcategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icono",
                table: "Subcategorias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icono",
                table: "Subcategorias");
        }
    }
}
