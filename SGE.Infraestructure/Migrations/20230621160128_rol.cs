using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGE.Infraestructure.Migrations
{
    public partial class rol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2023, 6, 21, 11, 1, 28, 774, DateTimeKind.Local).AddTicks(37));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Usuarios");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2023, 6, 13, 13, 41, 19, 467, DateTimeKind.Local).AddTicks(836));
        }
    }
}
