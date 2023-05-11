using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingPhotoAndAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "Syndicates",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "AppoinmentsDatesAvailable",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PHD",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Syndicates",
                table: "Doctors",
                newName: "SubSpeciatlity");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Doctors",
                newName: "Education");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Labs",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationFrom",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationTo",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Doctors",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "DurationFrom",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DurationTo",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "SubSpeciatlity",
                table: "Doctors",
                newName: "Syndicates");

            migrationBuilder.RenameColumn(
                name: "Education",
                table: "Doctors",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Syndicates",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppoinmentsDatesAvailable",
                table: "Doctors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Doctors",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PHD",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
