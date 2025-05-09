using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRegistrationFeesTablesReprocessorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 85, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 9 },
                    { 86, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 10 },
                    { 87, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 11 },
                    { 88, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 12 },
                    { 89, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 13 },
                    { 90, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 1, 14 },
                    { 91, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 9 },
                    { 92, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 10 },
                    { 93, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 11 },
                    { 94, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 12 },
                    { 95, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 13 },
                    { 96, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 2, 14 },
                    { 97, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 9 },
                    { 98, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 10 },
                    { 99, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 11 },
                    { 100, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 12 },
                    { 101, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 13 },
                    { 102, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 3, 14 },
                    { 103, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 9 },
                    { 104, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 10 },
                    { 105, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 11 },
                    { 106, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 12 },
                    { 107, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 13 },
                    { 108, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 8, 4, 14 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 108);
        }
    }
}
