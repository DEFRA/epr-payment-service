using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineMarketToProducer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 1, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 1, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 1, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 1, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 165800m, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 165800m, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 165800m, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 165800m, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 2, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 2, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 2, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 257900m, 2, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 3, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 3, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 3, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 3, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 55800m, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 4, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 4, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 4, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 14000m, 4, 6 });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 45, 71400m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 1, 7 },
                    { 46, 71400m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 2, 7 },
                    { 47, 71400m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 3, 7 },
                    { 48, 71400m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 4, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 165800m, 2, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 165800m, 2, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 165800m, 2, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 165800m, 2, 1 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 63100m, 2 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 1380400m, 3 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 257900m, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 257900m, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 257900m, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 257900m, 4 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 3, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 3, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 3, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 3, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 4, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 4, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 4, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 55800m, 4, 5 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Amount", "SubGroupId" },
                values: new object[] { 14000m, 6 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 71400m, 5, 7 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 71400m, 5, 7 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 71400m, 5, 7 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Amount", "GroupId", "SubGroupId" },
                values: new object[] { 71400m, 5, 7 });
        }
    }
}
