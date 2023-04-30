using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingNulls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "Doctors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePhoto",
                table: "Doctors",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
