using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRegistrationFeesTablesExporterData : Migration
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
                    { 61, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 9 },
                    { 62, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 10 },
                    { 63, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 11 },
                    { 64, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 12 },
                    { 65, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 13 },
                    { 66, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 1, 14 },
                    { 67, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 9 },
                    { 68, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 10 },
                    { 69, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 11 },
                    { 70, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 12 },
                    { 71, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 13 },
                    { 72, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 2, 14 },
                    { 73, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 9 },
                    { 74, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 10 },
                    { 75, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 11 },
                    { 76, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 12 },
                    { 77, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 13 },
                    { 78, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 3, 14 },
                    { 79, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 9 },
                    { 80, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 10 },
                    { 81, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 11 },
                    { 82, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 12 },
                    { 83, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 13 },
                    { 84, 2921m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 7, 4, 14 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 84);
        }
    }
}
