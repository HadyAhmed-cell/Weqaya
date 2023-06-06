using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingPatientTestsOcr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OcrCheck",
                table: "PatientTestsAndRisks");

            migrationBuilder.CreateTable(
                name: "PatientTestsOrRisksOcrs",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TestTestsAndRisksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTestsOrRisksOcrs", x => new { x.PatientId, x.TestTestsAndRisksId });
                    table.ForeignKey(
                        name: "FK_PatientTestsOrRisksOcrs_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientTestsOrRisksOcrs_testsAndRisks_TestTestsAndRisksId",
                        column: x => x.TestTestsAndRisksId,
                        principalTable: "testsAndRisks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientTestsOrRisksOcrs_TestTestsAndRisksId",
                table: "PatientTestsOrRisksOcrs",
                column: "TestTestsAndRisksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientTestsOrRisksOcrs");

            migrationBuilder.AddColumn<bool>(
                name: "OcrCheck",
                table: "PatientTestsAndRisks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}