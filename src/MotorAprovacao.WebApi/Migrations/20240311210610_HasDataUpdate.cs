using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorAprovacao.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class HasDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "RefundDocuments",
                type: "numeric(5)",
                precision: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,0)",
                oldPrecision: 5);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "RefundDocuments",
                type: "numeric(5,0)",
                precision: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5)",
                oldPrecision: 5);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 0);
        }
    }
}
