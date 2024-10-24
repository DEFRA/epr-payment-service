using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class ComplianceSchemeResubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Group",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[] { 6, "Compliance Scheme re-submitting a report", "ComplianceSchemeResubmission" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 57, 43000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 1, 7 },
                    { 58, 43000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 2, 7 },
                    { 59, 43000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 3, 7 },
                    { 60, 43000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 4, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Group",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
