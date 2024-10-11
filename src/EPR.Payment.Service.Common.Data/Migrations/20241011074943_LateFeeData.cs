using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class LateFeeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubGroup",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[] { 8, "Late Fee", "LateFee" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 49, 33200m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 1, 8 },
                    { 50, 33200m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 2, 8 },
                    { 51, 33200m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 3, 8 },
                    { 52, 33200m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 4, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
