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
                    Id_budynku = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budynek", x => x.Id_budynku);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    DataOfStudiesStart = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    BudynekId_budynku = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zajecia", x => x.Id_zajec);
                    table.ForeignKey(
                        name: "FK_Zajecia_Budynek_BudynekId_budynku",
                        column: x => x.BudynekId_budynku,
                        principalTable: "Budynek",
                        principalColumn: "Id_budynku");
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
                name: "Student_Zajecia",
                columns: table => new
                {
                    Student_Zajecia_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    zajeciaId_zajec = table.Column<int>(type: "INTEGER", nullable: true),
                    studentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_Zajecia", x => x.Student_Zajecia_ID);
                    table.ForeignKey(
                        name: "FK_Student_Zajecia_Student_studentId",
                        column: x => x.studentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Student_Zajecia_Zajecia_zajeciaId_zajec",
                        column: x => x.zajeciaId_zajec,
                        principalTable: "Zajecia",
                        principalColumn: "Id_zajec");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pracownik_ZajeciaId_zajec",
                table: "Pracownik",
                column: "ZajeciaId_zajec");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Zajecia_studentId",
                table: "Student_Zajecia",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Zajecia_zajeciaId_zajec",
                table: "Student_Zajecia",
                column: "zajeciaId_zajec");

            migrationBuilder.CreateIndex(
                name: "IX_Zajecia_BudynekId_budynku",
                table: "Zajecia",
                column: "BudynekId_budynku");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pracownik");

            migrationBuilder.DropTable(
                name: "Student_Zajecia");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Zajecia");

            migrationBuilder.DropTable(
                name: "Budynek");
        }
    }
}
