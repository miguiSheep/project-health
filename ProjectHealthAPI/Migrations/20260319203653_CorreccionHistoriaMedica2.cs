using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionHistoriaMedica2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Framaco",
                table: "HistoriasMedicas",
                newName: "Farmaco");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Farmaco",
                table: "HistoriasMedicas",
                newName: "Framaco");
        }
    }
}
