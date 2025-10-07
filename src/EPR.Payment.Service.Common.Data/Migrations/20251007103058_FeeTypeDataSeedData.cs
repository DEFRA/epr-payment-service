using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeeTypeDataSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Compliance Scheme Resubmission");

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FeeTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "FeePreviousPayment" },
                    { 12, "OutstandingPayment" },
                    { 13, "BandNumber 1" },
                    { 14, "BandNumber 2" },
                    { 15, "BandNumber 3" },
                    { 16, "PreviousPayment(reuse)" },
                    { 17, "OutstandingPayment(reuse)" },
                    { 18, "Member OnlineMarketPlace Fee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Compliance Scheme Resubmission Fee");
        }
    }
}
