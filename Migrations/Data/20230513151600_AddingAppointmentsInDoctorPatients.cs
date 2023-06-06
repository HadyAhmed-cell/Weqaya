using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingAppointmentsInDoctorPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "DoctorPatients",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "DoctorPatients");
        }
    }
}