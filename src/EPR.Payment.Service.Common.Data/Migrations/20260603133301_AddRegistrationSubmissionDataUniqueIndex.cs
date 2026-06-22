using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationSubmissionDataUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ;WITH dupes AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (
                               PARTITION BY SubmissionId, FileId
                               ORDER BY CreatedDate DESC
                           ) AS rn
                    FROM RegistrationSubmissionData
                )
                DELETE rsd
                FROM RegistrationSubmissionData rsd
                INNER JOIN dupes d ON rsd.Id = d.Id
                WHERE d.rn > 1;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionData_SubmissionId_FileId",
                table: "RegistrationSubmissionData",
                columns: new[] { "SubmissionId", "FileId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrationSubmissionData_SubmissionId_FileId",
                table: "RegistrationSubmissionData");
        }
    }
}
