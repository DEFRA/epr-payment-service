using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SUB399add2024submissionperiods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubmissionPeriod",
                columns: new[] { "Id", "ClosingDate", "DeadlineDate", "OpeningDate", "RegistrationYear", "WindowType" },
                values: new object[,]
                {
                    { 11, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, "Cso" },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, "Direct" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubmissionPeriod",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SubmissionPeriod",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
