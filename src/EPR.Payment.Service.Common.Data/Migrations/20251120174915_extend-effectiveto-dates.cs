using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class extendeffectivetodates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FeeTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 9, "Producer Resubmission Fee" },
                    { 10, "Compliance Scheme Resubmission" }
                });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 1,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 2,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 3,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 4,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 5,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 6,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 7,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 8,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 41,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 42,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 43,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 44,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 45,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 46,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 47,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 48,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 49,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 50,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 51,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 52,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 53,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 54,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 55,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 56,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 57,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 58,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 59,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 60,
                column: "EffectiveTo",
                value: new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 1,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 2,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 3,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 4,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 5,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 6,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 7,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 8,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 41,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 42,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 43,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 44,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 45,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 46,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 47,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 48,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 49,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 50,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 51,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 52,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 53,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 54,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 55,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 56,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 57,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 58,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 59,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 60,
                column: "EffectiveTo",
                value: new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc));
        }
    }
}
