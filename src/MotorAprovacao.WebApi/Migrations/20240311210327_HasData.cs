using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorAprovacao.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class HasData : Migration
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Outros" },
                    { 2, "Alimentação" },
                    { 3, "Transporte" }
                });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "Id", "CategoryId", "MaximumToApprove", "MinimumToDisapprove" },
                values: new object[,]
                {
                    { 1, 0, 100m, 1000m },
                    { 2, 0, 500m, 1000m },
                    { 3, 0, 500m, 1000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "RefundDocuments",
                type: "numeric(5,0)",
                precision: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5)",
                oldPrecision: 5);
        }
    }
}
