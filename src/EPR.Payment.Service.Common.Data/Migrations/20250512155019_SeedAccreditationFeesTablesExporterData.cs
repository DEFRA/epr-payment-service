using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccreditationFeesTablesExporterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "AccreditationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "FeesPerSite", "GroupId", "RegulatorId", "SubGroupId", "TonnesOver", "TonnesUpTo" },
                values: new object[,]
                {
                    { 1, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 9, 0, 500 },
                    { 2, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 9, 500, 5000 },
                    { 3, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 9, 5000, 10000 },
                    { 4, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 9, 10000, 99999999 },
                    { 5, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 10, 0, 500 },
                    { 6, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 10, 500, 5000 },
                    { 7, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 10, 5000, 10000 },
                    { 8, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 10, 10000, 99999999 },
                    { 9, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 11, 0, 500 },
                    { 10, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 11, 500, 5000 },
                    { 11, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 11, 5000, 10000 },
                    { 12, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 11, 10000, 99999999 },
                    { 13, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 12, 0, 500 },
                    { 14, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 12, 500, 5000 },
                    { 15, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 12, 5000, 10000 },
                    { 16, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 12, 10000, 99999999 },
                    { 17, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 13, 0, 500 },
                    { 18, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 13, 500, 5000 },
                    { 19, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 13, 5000, 10000 },
                    { 20, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 13, 10000, 99999999 },
                    { 21, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 14, 0, 500 },
                    { 22, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 14, 500, 5000 },
                    { 23, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 14, 5000, 10000 },
                    { 24, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 1, 14, 10000, 99999999 },
                    { 25, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 9, 0, 500 },
                    { 26, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 9, 500, 5000 },
                    { 27, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 9, 5000, 10000 },
                    { 28, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 9, 10000, 99999999 },
                    { 29, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 10, 0, 500 },
                    { 30, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 10, 500, 5000 },
                    { 31, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 10, 5000, 10000 },
                    { 32, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 10, 10000, 99999999 },
                    { 33, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 11, 0, 500 },
                    { 34, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 11, 500, 5000 },
                    { 35, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 11, 5000, 10000 },
                    { 36, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 11, 10000, 99999999 },
                    { 37, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 12, 0, 500 },
                    { 38, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 12, 500, 5000 },
                    { 39, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 12, 5000, 10000 },
                    { 40, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 12, 10000, 99999999 },
                    { 41, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 13, 0, 500 },
                    { 42, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 13, 500, 5000 },
                    { 43, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 13, 5000, 10000 },
                    { 44, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 13, 10000, 99999999 },
                    { 45, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 14, 0, 500 },
                    { 46, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 14, 500, 5000 },
                    { 47, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 14, 5000, 10000 },
                    { 48, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 4, 14, 10000, 99999999 },
                    { 49, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 9, 0, 500 },
                    { 50, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 9, 500, 5000 },
                    { 51, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 9, 5000, 10000 },
                    { 52, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 9, 10000, 99999999 },
                    { 53, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 10, 0, 500 },
                    { 54, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 10, 500, 5000 },
                    { 55, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 10, 5000, 10000 },
                    { 56, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 10, 10000, 99999999 },
                    { 57, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 11, 0, 500 },
                    { 58, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 11, 500, 5000 },
                    { 59, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 11, 5000, 10000 },
                    { 60, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 11, 10000, 99999999 },
                    { 61, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 12, 0, 500 },
                    { 62, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 12, 500, 5000 },
                    { 63, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 12, 5000, 10000 },
                    { 64, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 12, 10000, 99999999 },
                    { 65, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 13, 0, 500 },
                    { 66, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 13, 500, 5000 },
                    { 67, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 13, 5000, 10000 },
                    { 68, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 13, 10000, 99999999 },
                    { 69, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 14, 0, 500 },
                    { 70, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 14, 500, 5000 },
                    { 71, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 14, 5000, 10000 },
                    { 72, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 2, 14, 10000, 99999999 },
                    { 73, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 9, 0, 500 },
                    { 74, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 9, 500, 5000 },
                    { 75, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 9, 5000, 10000 },
                    { 76, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 9, 10000, 99999999 },
                    { 77, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 10, 0, 500 },
                    { 78, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 10, 500, 5000 },
                    { 79, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 10, 5000, 10000 },
                    { 80, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 10, 10000, 99999999 },
                    { 81, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 11, 0, 500 },
                    { 82, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 11, 500, 5000 },
                    { 83, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 11, 5000, 10000 },
                    { 84, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 11, 10000, 99999999 },
                    { 85, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 12, 0, 500 },
                    { 86, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 12, 500, 5000 },
                    { 87, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 12, 5000, 10000 },
                    { 88, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 12, 10000, 99999999 },
                    { 89, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 13, 0, 500 },
                    { 90, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 13, 500, 5000 },
                    { 91, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 13, 5000, 10000 },
                    { 92, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 13, 10000, 99999999 },
                    { 93, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 14, 0, 500 },
                    { 94, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 14, 500, 5000 },
                    { 95, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 14, 5000, 10000 },
                    { 96, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 216m, 7, 3, 14, 10000, 99999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 96);
        }
    }
}
