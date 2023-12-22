using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Migrations
{
    /// <inheritdoc />
    public partial class PateintConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Patient",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            //migrationBuilder.AlterColumn<byte[]>(
            //    name: "RowVersion",
            //    table: "Doctor",
            //    type: "rowversion",
            //    rowVersion: true,
            //    nullable: true,
            //    oldClrType: typeof(byte[]),
            //    oldType: "rowversion",
            //    oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Patient");

            //migrationBuilder.AlterColumn<byte[]>(
            //    name: "RowVersion",
            //    table: "Doctor",
            //    type: "rowversion",
            //    rowVersion: true,
            //    nullable: false,
            //    defaultValue: new byte[0],
            //    oldClrType: typeof(byte[]),
            //    oldType: "rowversion",
            //    oldRowVersion: true,
            //    oldNullable: true);
        }
    }
}
