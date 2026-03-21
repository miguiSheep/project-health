using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionHistoriaMedica1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paciente",
                table: "HistoriasMedicas");

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "HistoriasMedicas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasMedicas_PacienteId",
                table: "HistoriasMedicas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriasMedicas_Pacientes_PacienteId",
                table: "HistoriasMedicas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriasMedicas_Pacientes_PacienteId",
                table: "HistoriasMedicas");

            migrationBuilder.DropIndex(
                name: "IX_HistoriasMedicas_PacienteId",
                table: "HistoriasMedicas");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "HistoriasMedicas");

            migrationBuilder.AddColumn<string>(
                name: "Paciente",
                table: "HistoriasMedicas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
