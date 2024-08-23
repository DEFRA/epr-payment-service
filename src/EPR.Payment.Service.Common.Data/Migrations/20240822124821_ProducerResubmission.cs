using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProducerResubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Group",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[] { 5, "Producer re-submitting a report", "ProducerResubmission" });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 262000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 262000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 3,
                column: "Amount",
                value: 262000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 4,
                column: "Amount",
                value: 262000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 5,
                column: "Amount",
                value: 121600m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 6,
                column: "Amount",
                value: 121600m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 7,
                column: "Amount",
                value: 121600m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 8,
                column: "Amount",
                value: 121600m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 63100m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 63100m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 63100m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "Amount",
                value: 63100m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                column: "Amount",
                value: 1380400m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                column: "Amount",
                value: 1380400m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                column: "Amount",
                value: 1380400m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                column: "Amount",
                value: 1380400m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                column: "Amount",
                value: 257900m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                column: "Amount",
                value: 257900m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                column: "Amount",
                value: 257900m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                column: "Amount",
                value: 257900m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                column: "Amount",
                value: 55800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                column: "Amount",
                value: 14000m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                column: "Amount",
                value: 14000m);

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubGroup",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[] { 7, "Re-submitting a report", "ReSubmitting" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 41, 71400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 7 },
                    { 42, 71400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, 7 },
                    { 43, 71400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3, 7 },
                    { 44, 71400m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 4, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Group",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 2620m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Amount",
                value: 2620m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 3,
                column: "Amount",
                value: 2620m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 4,
                column: "Amount",
                value: 2620m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 5,
                column: "Amount",
                value: 1216m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 6,
                column: "Amount",
                value: 1216m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 7,
                column: "Amount",
                value: 1216m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 8,
                column: "Amount",
                value: 1216m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                column: "Amount",
                value: 1658m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                column: "Amount",
                value: 1658m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                column: "Amount",
                value: 1658m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                column: "Amount",
                value: 1658m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 631m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 631m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 631m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "Amount",
                value: 631m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                column: "Amount",
                value: 13804m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                column: "Amount",
                value: 13804m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                column: "Amount",
                value: 13804m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                column: "Amount",
                value: 13804m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                column: "Amount",
                value: 2579m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                column: "Amount",
                value: 2579m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                column: "Amount",
                value: 2579m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                column: "Amount",
                value: 2579m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                column: "Amount",
                value: 558m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                column: "Amount",
                value: 140m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                column: "Amount",
                value: 140m);
        }
    }
}
