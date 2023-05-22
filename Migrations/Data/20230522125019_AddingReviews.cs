using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualClinic.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddingReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OcrCheck",
                table: "PatientTestsAndRisks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabReviews",
                table: "LabReviews",
                columns: new[] { "LabId", "PatientId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorReviews",
                table: "DoctorReviews",
                columns: new[] { "DoctorId", "PatientId" });

            migrationBuilder.CreateIndex(
                name: "IX_LabReviews_PatientId",
                table: "LabReviews",
                column: "PatientId");

            migrationBuilder.CreateIndex(
    name: "IX_LabReviews_LabId",
    table: "LabReviews",
    column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReviews_PatientId",
                table: "DoctorReviews",
                column: "PatientId");
            migrationBuilder.CreateIndex(
                name: "IX_DoctorReviews_DoctorId",
                table: "DoctorReviews",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorReviews_Doctors_DoctorId",
                table: "DoctorReviews",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
               name: "FK_LabReviews_Labs_LabId",
               table: "LabReviews",
               column: "LabId",
               principalTable: "Labs",
               principalColumn: "Id",
               onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorReviews_Patients_PatientId",
                table: "DoctorReviews",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabReviews_Patients_PatientId",
                table: "LabReviews",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorReviews_Patients_PatientId",
                table: "DoctorReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_LabReviews_Patients_PatientId",
                table: "LabReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabReviews",
                table: "LabReviews");

            migrationBuilder.DropIndex(
                name: "IX_LabReviews_PatientId",
                table: "LabReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorReviews",
                table: "DoctorReviews");

            migrationBuilder.DropIndex(
                name: "IX_DoctorReviews_PatientId",
                table: "DoctorReviews");

            migrationBuilder.DropColumn(
                name: "OcrCheck",
                table: "PatientTestsAndRisks");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "LabReviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "DoctorReviews",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LabReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoctorReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabReviews",
                table: "LabReviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorReviews",
                table: "DoctorReviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LabReviews_LabId",
                table: "LabReviews",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReviews_DoctorId",
                table: "DoctorReviews",
                column: "DoctorId");
        }
    }
}