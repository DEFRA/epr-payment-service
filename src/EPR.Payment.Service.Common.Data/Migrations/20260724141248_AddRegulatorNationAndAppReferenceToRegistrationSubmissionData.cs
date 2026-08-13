using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegulatorNationAndAppReferenceToRegistrationSubmissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing registration submission rows so the new NOT NULL columns
            // don't need a placeholder empty-string default that would silently break fee
            // calculation. Children first to satisfy existing FKs. Mirrors the pattern
            // established by 20260709105007_AddSubmissionPeriodAndLink.
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionDataEvents];");
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionSubsidiary];");
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionProducer];");
            migrationBuilder.Sql("DELETE FROM [registration].[RegistrationSubmissionData];");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationReferenceNumber",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegulatorNation",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationReferenceNumber",
                schema: "registration",
                table: "RegistrationSubmissionData");

            migrationBuilder.DropColumn(
                name: "RegulatorNation",
                schema: "registration",
                table: "RegistrationSubmissionData");
        }
    }
}
