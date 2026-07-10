using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionPeriodAndLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing registration submission rows so all future data is populated
            // against the new SubmissionPeriodId FK. Children first to satisfy existing FKs.
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionSubsidiary];");
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionProducer];");
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionData];");

            migrationBuilder.DropColumn(
                name: "SubmissionPeriod",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.AddColumn<int>(
                name: "SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubmissionPeriod",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WindowType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RegistrationYear = table.Column<int>(type: "int", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeadlineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionPeriod", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubmissionPeriod",
                columns: new[] { "Id", "ClosingDate", "DeadlineDate", "OpeningDate", "RegistrationYear", "WindowType" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2025, "Cso" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2025, "Direct" },
                    { 3, new DateTime(2027, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, "CsoLargeProducer" },
                    { 4, new DateTime(2027, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, "CsoSmallProducer" },
                    { 5, new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, "DirectLargeProducer" },
                    { 6, new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, "DirectSmallProducer" },
                    { 7, new DateTime(2028, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2027, "CsoLargeProducer" },
                    { 8, new DateTime(2028, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2027, "CsoSmallProducer" },
                    { 9, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2027, "DirectLargeProducer" },
                    { 10, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2027, "DirectSmallProducer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionData_SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                column: "SubmissionPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionPeriod_WindowType_RegistrationYear",
                schema: "Lookup",
                table: "SubmissionPeriod",
                columns: new[] { "WindowType", "RegistrationYear" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationSubmissionData_SubmissionPeriod_SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                column: "SubmissionPeriodId",
                principalSchema: "Lookup",
                principalTable: "SubmissionPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationSubmissionData_SubmissionPeriod_SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.DropTable(
                name: "SubmissionPeriod",
                schema: "Lookup");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationSubmissionData_SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.DropColumn(
                name: "SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.AddColumn<string>(
                name: "SubmissionPeriod",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
