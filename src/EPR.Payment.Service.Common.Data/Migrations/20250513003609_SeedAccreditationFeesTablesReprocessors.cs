using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccreditationFeesTablesReprocessors : Migration
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
                    { 97, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 9, 0, 500 },
                    { 98, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 9, 500, 5000 },
                    { 99, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 9, 5000, 10000 },
                    { 100, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 9, 10000, 99999999 },
                    { 101, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 10, 0, 500 },
                    { 102, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 10, 500, 5000 },
                    { 103, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 10, 5000, 10000 },
                    { 104, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 10, 10000, 99999999 },
                    { 105, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 11, 0, 500 },
                    { 106, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 11, 500, 5000 },
                    { 107, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 11, 5000, 10000 },
                    { 108, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 11, 10000, 99999999 },
                    { 109, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 12, 0, 500 },
                    { 110, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 12, 500, 5000 },
                    { 111, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 12, 5000, 10000 },
                    { 112, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 12, 10000, 99999999 },
                    { 113, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 13, 0, 500 },
                    { 114, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 13, 500, 5000 },
                    { 115, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 13, 5000, 10000 },
                    { 116, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 13, 10000, 99999999 },
                    { 117, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 14, 0, 500 },
                    { 118, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 14, 500, 5000 },
                    { 119, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 14, 5000, 10000 },
                    { 120, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 1, 14, 10000, 99999999 },
                    { 121, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 9, 0, 500 },
                    { 122, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 9, 500, 5000 },
                    { 123, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 9, 5000, 10000 },
                    { 124, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 9, 10000, 99999999 },
                    { 125, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 10, 0, 500 },
                    { 126, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 10, 500, 5000 },
                    { 127, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 10, 5000, 10000 },
                    { 128, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 10, 10000, 99999999 },
                    { 129, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 11, 0, 500 },
                    { 130, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 11, 500, 5000 },
                    { 131, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 11, 5000, 10000 },
                    { 132, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 11, 10000, 99999999 },
                    { 133, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 12, 0, 500 },
                    { 134, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 12, 500, 5000 },
                    { 135, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 12, 5000, 10000 },
                    { 136, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 12, 10000, 99999999 },
                    { 137, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 13, 0, 500 },
                    { 138, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 13, 500, 5000 },
                    { 139, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 13, 5000, 10000 },
                    { 140, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 13, 10000, 99999999 },
                    { 141, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 14, 0, 500 },
                    { 142, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 14, 500, 5000 },
                    { 143, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 14, 5000, 10000 },
                    { 144, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 4, 14, 10000, 99999999 },
                    { 145, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 9, 0, 500 },
                    { 146, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 9, 500, 5000 },
                    { 147, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 9, 5000, 10000 },
                    { 148, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 9, 10000, 99999999 },
                    { 149, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 10, 0, 500 },
                    { 150, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 10, 500, 5000 },
                    { 151, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 10, 5000, 10000 },
                    { 152, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 10, 10000, 99999999 },
                    { 153, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 11, 0, 500 },
                    { 154, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 11, 500, 5000 },
                    { 155, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 11, 5000, 10000 },
                    { 156, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 11, 10000, 99999999 },
                    { 157, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 12, 0, 500 },
                    { 158, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 12, 500, 5000 },
                    { 159, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 12, 5000, 10000 },
                    { 160, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 12, 10000, 99999999 },
                    { 161, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 13, 0, 500 },
                    { 162, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 13, 500, 5000 },
                    { 163, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 13, 5000, 10000 },
                    { 164, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 13, 10000, 99999999 },
                    { 165, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 14, 0, 500 },
                    { 166, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 14, 500, 5000 },
                    { 167, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 14, 5000, 10000 },
                    { 168, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 2, 14, 10000, 99999999 },
                    { 169, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 9, 0, 500 },
                    { 170, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 9, 500, 5000 },
                    { 171, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 9, 5000, 10000 },
                    { 172, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 9, 10000, 99999999 },
                    { 173, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 10, 0, 500 },
                    { 174, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 10, 500, 5000 },
                    { 175, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 10, 5000, 10000 },
                    { 176, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 10, 10000, 99999999 },
                    { 177, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 11, 0, 500 },
                    { 178, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 11, 500, 5000 },
                    { 179, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 11, 5000, 10000 },
                    { 180, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 11, 10000, 99999999 },
                    { 181, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 12, 0, 500 },
                    { 182, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 12, 500, 5000 },
                    { 183, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 12, 5000, 10000 },
                    { 184, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 12, 10000, 99999999 },
                    { 185, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 13, 0, 500 },
                    { 186, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 13, 500, 5000 },
                    { 187, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 13, 5000, 10000 },
                    { 188, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 13, 10000, 99999999 },
                    { 189, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 14, 0, 500 },
                    { 190, 2000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 14, 500, 5000 },
                    { 191, 3000m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 14, 5000, 10000 },
                    { 192, 3631m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 8, 31, 23, 59, 59, 0, DateTimeKind.Utc), 0m, 8, 3, 14, 10000, 99999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 192);
        }
    }
}
