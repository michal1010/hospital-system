using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Migrations
{
    /// <inheritdoc />
    public partial class ShiftAtributesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Shift",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Shift",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shift_PatientId",
                table: "Shift",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Patient_PatientId",
                table: "Shift",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Patient_PatientId",
                table: "Shift");

            migrationBuilder.DropIndex(
                name: "IX_Shift_PatientId",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Shift");
        }
    }
}
