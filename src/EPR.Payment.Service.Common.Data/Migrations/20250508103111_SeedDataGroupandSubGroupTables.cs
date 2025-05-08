using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataGroupandSubGroupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Group",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 7, "Exporters", "Exporters" },
                    { 8, "Reprocessors", "Reprocessors" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubGroup",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 9, "Aluminium", "Aluminium" },
                    { 10, "Glass", "Glass" },
                    { 11, "Paper, board or fibre-based composite material", "PaperOrBoardOrFibreBasedCompositeMaterial" },
                    { 12, "Plastic", "Plastic" },
                    { 13, "Steel", "Steel" },
                    { 14, "Wood", "Wood" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Group",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Group",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
