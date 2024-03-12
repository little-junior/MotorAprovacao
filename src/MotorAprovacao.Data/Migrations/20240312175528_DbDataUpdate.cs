using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorAprovacao.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DbDataUpdate : Migration
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
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Alimentacao");
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
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Hospedagem");
        }
    }
}
