using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "DoctorPatients",
    columns: table => new
    {
        doctorId = table.Column<int>(type: "int", nullable: false),
        patientId = table.Column<int>(type: "int", nullable: false),
        AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
        StatusNum = table.Column<int>(type: "int", nullable: false),
        DoctorNotes = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "No Notes Yet!"),
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_DoctorPatients", x => new { x.doctorId, x.patientId });
        table.ForeignKey(
            name: "FK_DoctorPatients_Doctors_doctorId",
            column: x => x.doctorId,
            principalTable: "Doctors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_DoctorPatients_Patients_patientId",
            column: x => x.patientId,
            principalTable: "Patients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
            migrationBuilder.CreateIndex(
    name: "IX_DoctorPatients_patientId",
    table: "DoctorPatients",
    column: "patientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}