using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFileIdFromRegistrationSubmissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrationSubmissionData_SubmissionId_FileId",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.DropColumn(
                name: "FileId",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.Sql(@"
                ;WITH dupes AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (
                               PARTITION BY RegistrationBlobName
                               ORDER BY CreatedDate DESC
                           ) AS rn
                    FROM [registration].[RegistrationSubmissionData]
                )
                DELETE rsd
                FROM [registration].[RegistrationSubmissionData] rsd
                INNER JOIN dupes d ON rsd.Id = d.Id
                WHERE d.rn > 1;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionData_RegistrationBlobName",
                schema: "registration",
                table: "RegistrationSubmissionData",
                column: "RegistrationBlobName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrationSubmissionData_RegistrationBlobName",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionData_SubmissionId_FileId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                columns: new[] { "SubmissionId", "FileId" },
                unique: true);
        }
    }
}
