using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class EditingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_GeoLocation_GeoLocationId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Labs_GeoLocation_GeoLocationId",
                table: "Labs");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_GeoLocation_GeoLocationId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "GeoLocation");

            migrationBuilder.DropTable(
                name: "SocialStatus");

            migrationBuilder.DropIndex(
                name: "IX_Patients_GeoLocationId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Labs_GeoLocationId",
                table: "Labs");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_GeoLocationId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "GeoLocationId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "GeoLocationId",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "GeoLocationId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "GeoLocationId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeoLocationId",
                table: "Labs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeoLocationId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeoLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lat = table.Column<double>(type: "float", nullable: false),
                    lng = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_GeoLocationId",
                table: "Patients",
                column: "GeoLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Labs_GeoLocationId",
                table: "Labs",
                column: "GeoLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_GeoLocationId",
                table: "Doctors",
                column: "GeoLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_GeoLocation_GeoLocationId",
                table: "Doctors",
                column: "GeoLocationId",
                principalTable: "GeoLocation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Labs_GeoLocation_GeoLocationId",
                table: "Labs",
                column: "GeoLocationId",
                principalTable: "GeoLocation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_GeoLocation_GeoLocationId",
                table: "Patients",
                column: "GeoLocationId",
                principalTable: "GeoLocation",
                principalColumn: "Id");
        }
    }
}
