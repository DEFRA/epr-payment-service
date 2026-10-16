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

            // Known failures from prod run BE4B361F-1F5B-498E-AB33-1E3922D89527. Seeded here rather than via HasData
            // so rows added later with Scripts/Migration/exclude-submissions.sql are not owned by the model.
            migrationBuilder.InsertData(
                schema: "migration",
                table: "ExcludedSubmission",
                columns: new[] { "SubmissionId", "Reason", "Note" },
                values: new object[,]
                {
                    { "ea0be992-d316-410a-9235-2e9d024cb79d", "CsvInvalid", "Compliance scheme file has subsidiary rows for OrganisationId 101002 but no parent row (row validation did not run on upload)." },
                    { "0341aca8-240c-40c5-9080-83b2689d2a48", "CsvInvalid", "Compliance scheme file has subsidiary rows for OrganisationId 101594 but no parent row (row validation did not run on upload)." },
                    { "f09c6273-e322-4b9b-8d21-0ae7800bfbdd", "CsvInvalid", "Compliance scheme file has subsidiary rows for OrganisationId 115537 but no parent row (row validation did not run on upload)." },
                    { "28ba0d67-3b18-4704-92e4-a2ddd9c8de99", "CsvInvalid", "Compliance scheme file has subsidiary rows for OrganisationId 101877 but no parent row (row validation did not run on upload)." },
                    { "2b8337bb-34aa-4a86-ad47-5a9aebbd07a3", "ArnMissing", "File e6f91b8b-5d65-442c-a081-e6e868211612: no application reference number recorded in Synapse." },
                    { "579c33d6-034a-45d7-b3e5-523b58eb90b3", "ArnMissing", "File 1bb2cb86-fe76-42bf-a553-1d1019a92e80: no application reference number recorded in Synapse." },
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
