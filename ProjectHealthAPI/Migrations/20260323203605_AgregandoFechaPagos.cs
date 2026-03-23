using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoFechaPagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Referencia",
                table: "Pagos",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Fecha",
                table: "Pagos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Pagos");

            migrationBuilder.AlterColumn<int>(
                name: "Referencia",
                table: "Pagos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
