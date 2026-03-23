using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class DTOsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ansioloiticos",
                table: "HistoriasMedicas",
                newName: "Ansioliticos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ansioliticos",
                table: "HistoriasMedicas",
                newName: "Ansioloiticos");
        }
    }
}
