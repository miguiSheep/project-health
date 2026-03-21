using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionHistoriaMedica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "Peso",
                table: "Pacientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Pacientes");

            migrationBuilder.AddColumn<int>(
                name: "Peso",
                table: "Clientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
