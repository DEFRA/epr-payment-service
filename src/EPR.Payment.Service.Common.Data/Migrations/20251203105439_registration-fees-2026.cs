using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class registrationfees2026 : Migration
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
                    { 26000000, 180300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 1, 1 },
                    { 26000001, 180300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 2, 1 },
                    { 26000002, 180300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 3, 1 },
                    { 26000003, 180300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 4, 1 },
                    { 26000004, 69600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 1, 2 },
                    { 26000005, 69600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 2, 2 },
                    { 26000006, 69600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 3, 2 },
                    { 26000007, 69600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 4, 2 },
                    { 26000008, 1470200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 1, 3 },
                    { 26000009, 1470200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 2, 3 },
                    { 26000010, 1470200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 3, 3 },
                    { 26000011, 1470200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 4, 3 },
                    { 26000012, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 1, 4 },
                    { 26000013, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 2, 4 },
                    { 26000014, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 3, 4 },
                    { 26000015, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 4, 4 },
                    { 26000016, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 1, 8 },
                    { 26000017, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 2, 8 },
                    { 26000018, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 3, 8 },
                    { 26000019, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 2, 4, 8 },
                    { 26000020, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 1, 5 },
                    { 26000021, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 2, 5 },
                    { 26000022, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 3, 5 },
                    { 26000023, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 4, 5 },
                    { 26000024, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 1, 6 },
                    { 26000025, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 2, 6 },
                    { 26000026, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 3, 6 },
                    { 26000027, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 4, 4, 6 },
                    { 26000028, 51200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 1, 7 },
                    { 26000029, 51200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 2, 7 },
                    { 26000030, 51200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 3, 7 },
                    { 26000031, 51200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 6, 4, 7 },
                    { 26000032, 284200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 1, 1 },
                    { 26000033, 284200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 2, 1 },
                    { 26000034, 284200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 3, 1 },
                    { 26000035, 284200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 4, 1 },
                    { 26000036, 130300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 1, 2 },
                    { 26000037, 130300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 2, 2 },
                    { 26000038, 130300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 3, 2 },
                    { 26000039, 130300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 4, 2 },
                    { 26000040, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 1, 4 },
                    { 26000041, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 2, 4 },
                    { 26000042, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 3, 4 },
                    { 26000043, 288500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 4, 4 },
                    { 26000044, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 1, 8 },
                    { 26000045, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 2, 8 },
                    { 26000046, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 3, 8 },
                    { 26000047, 38600m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 1, 4, 8 },
                    { 26000048, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 1, 5 },
                    { 26000049, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 2, 5 },
                    { 26000050, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 3, 5 },
                    { 26000051, 69000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 4, 5 },
                    { 26000052, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 1, 6 },
                    { 26000053, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 2, 6 },
                    { 26000054, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 3, 6 },
                    { 26000055, 17200m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 3, 4, 6 },
                    { 26000056, 80700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 1, 7 },
                    { 26000057, 80700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 2, 7 },
                    { 26000058, 80700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 3, 7 },
                    { 26000059, 80700m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), 5, 4, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000000);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000001);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000002);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000003);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000004);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000005);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000006);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000007);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000008);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000009);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000010);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000011);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000012);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000013);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000014);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000015);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000016);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000017);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000018);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000019);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000020);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000021);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000022);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000023);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000024);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000025);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000026);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000027);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000028);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000029);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000030);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000031);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000032);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000033);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000034);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000035);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000036);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000037);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000038);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000039);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000040);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000041);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000042);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000043);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000044);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000045);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000046);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000047);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000048);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000049);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000050);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000051);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000052);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000053);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000054);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000055);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000056);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000057);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000058);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 26000059);
        }
    }
}
