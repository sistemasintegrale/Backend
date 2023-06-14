using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGE.Infraestructure.Migrations
{
    public partial class codigosCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    FechaCreacion = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "getdate()"),
                    FechaModificacion = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    FechaEliminacion = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CodigoClienteNG = table.Column<int>(type: "int", nullable: false),
                    CodigoClienteNM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellidos", "CodigoClienteNG", "CodigoClienteNM", "Email", "FechaCreacion", "FechaEliminacion", "FechaModificacion", "Nombre", "Password" },
                values: new object[] { 1, "", 0, 0, "side@gmail.com", new DateTime(2023, 6, 13, 13, 41, 19, 467, DateTimeKind.Local).AddTicks(836), null, null, "SISTEMA", "cgBvAGcAbwBsAGEAMgAwADEAMgA=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
