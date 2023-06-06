using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class EditingTablesPatientsDoctors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NoOfKids",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "SocialStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Syndicates",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "WaistDiameter",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Syndicates",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Syndicates",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Syndicates",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "Syndicates",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfKids",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialStatus",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Syndicates",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaistDiameter",
                table: "Patients",
                type: "int",
                nullable: true);
        }
    }
}