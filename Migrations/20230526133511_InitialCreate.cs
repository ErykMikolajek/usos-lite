using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab10.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budynek",
                columns: table => new
                {
                    Id_zajec = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budynek", x => x.Id_zajec);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    DataZatrudnienia = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zajecia",
                columns: table => new
                {
                    Id_zajec = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    BudynekId_zajec = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zajecia", x => x.Id_zajec);
                    table.ForeignKey(
                        name: "FK_Zajecia_Budynek_BudynekId_zajec",
                        column: x => x.BudynekId_zajec,
                        principalTable: "Budynek",
                        principalColumn: "Id_zajec");
                });

            migrationBuilder.CreateTable(
                name: "Pracownik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    DataZatrudnienia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ZajeciaId_zajec = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pracownik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pracownik_Zajecia_ZajeciaId_zajec",
                        column: x => x.ZajeciaId_zajec,
                        principalTable: "Zajecia",
                        principalColumn: "Id_zajec");
                });

            migrationBuilder.CreateTable(
                name: "StudentZajecia",
                columns: table => new
                {
                    StudenciId = table.Column<int>(type: "INTEGER", nullable: false),
                    ZajeciaId_zajec = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentZajecia", x => new { x.StudenciId, x.ZajeciaId_zajec });
                    table.ForeignKey(
                        name: "FK_StudentZajecia_Student_StudenciId",
                        column: x => x.StudenciId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentZajecia_Zajecia_ZajeciaId_zajec",
                        column: x => x.ZajeciaId_zajec,
                        principalTable: "Zajecia",
                        principalColumn: "Id_zajec",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pracownik_ZajeciaId_zajec",
                table: "Pracownik",
                column: "ZajeciaId_zajec");

            migrationBuilder.CreateIndex(
                name: "IX_StudentZajecia_ZajeciaId_zajec",
                table: "StudentZajecia",
                column: "ZajeciaId_zajec");

            migrationBuilder.CreateIndex(
                name: "IX_Zajecia_BudynekId_zajec",
                table: "Zajecia",
                column: "BudynekId_zajec");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pracownik");

            migrationBuilder.DropTable(
                name: "StudentZajecia");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Zajecia");

            migrationBuilder.DropTable(
                name: "Budynek");
        }
    }
}
