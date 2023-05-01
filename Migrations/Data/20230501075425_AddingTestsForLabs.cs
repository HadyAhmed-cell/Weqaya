using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingTestsForLabs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "LabsTestsAndRisks",
                columns: table => new
                {
                    LabId = table.Column<int>(type: "int", nullable: false),
                    TestsAndRisksId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabsTestsAndRisks", x => new { x.LabId, x.TestsAndRisksId });
                    table.ForeignKey(
                        name: "FK_LabsTestsAndRisks_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabsTestsAndRisks_testsAndRisks_TestsAndRisksId",
                        column: x => x.TestsAndRisksId,
                        principalTable: "testsAndRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

 
            migrationBuilder.CreateIndex(
                name: "IX_LabsTestsAndRisks_TestsAndRisksId",
                table: "LabsTestsAndRisks",
                column: "TestsAndRisksId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorPatients");

            migrationBuilder.DropTable(
                name: "LabPatients");

            migrationBuilder.DropTable(
                name: "LabsTestsAndRisks");

            migrationBuilder.DropTable(
                name: "PatientTestsAndRisks");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Labs");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "testsAndRisks");
        }
    }
}
