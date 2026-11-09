using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SUB365addmigrationexcludedsubmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExcludedCount",
                schema: "migration",
                table: "MigrationRun",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExcludedSubmission",
                schema: "migration",
                columns: table => new
                {
                    SubmissionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludedSubmission", x => x.SubmissionId);
                });

            // Known failure from prod run BE4B361F-1F5B-498E-AB33-1E3922D89527 that cannot be migrated. Seeded here rather
            // than via HasData so rows added later with Scripts/Migration/exclude-submissions.sql are not owned by the model.
            // Other CsvInvalid submissions are migrated without their invalid files, and ArnMissing ones get an ARN derived
            // in the registration history dataset, so they are no longer excluded.
            migrationBuilder.InsertData(
                schema: "migration",
                table: "ExcludedSubmission",
                columns: new[] { "SubmissionId", "Reason", "Note" },
                values: new object[,]
                {
                    { "f09c6273-e322-4b9b-8d21-0ae7800bfbdd", "CsvInvalid", "Only file 37c0f3f5-cf86-4960-90cf-c41fbdf25028 has subsidiary rows for OrganisationId 115537 but no parent row (row validation did not run on upload); no application submitted." },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcludedSubmission",
                schema: "migration");

            migrationBuilder.DropColumn(
                name: "ExcludedCount",
                schema: "migration",
                table: "MigrationRun");
        }
    }
}
