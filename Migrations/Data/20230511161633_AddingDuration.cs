using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationTo",
                table: "Doctors",
                newName: "TimeTo");

            migrationBuilder.RenameColumn(
                name: "DurationFrom",
                table: "Doctors",
                newName: "TimeFrom");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Doctors",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "TimeTo",
                table: "Doctors",
                newName: "DurationTo");

            migrationBuilder.RenameColumn(
                name: "TimeFrom",
                table: "Doctors",
                newName: "DurationFrom");
        }
    }
}