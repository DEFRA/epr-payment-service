using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveRegistrationSubmissionTablesToRegistrationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "registration");

            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionSubsidiary",
                newName: "RegistrationSubmissionSubsidiary",
                newSchema: "registration");

            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionProducer",
                newName: "RegistrationSubmissionProducer",
                newSchema: "registration");

            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionData",
                newName: "RegistrationSubmissionData",
                newSchema: "registration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionSubsidiary",
                schema: "registration",
                newName: "RegistrationSubmissionSubsidiary");

            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionProducer",
                schema: "registration",
                newName: "RegistrationSubmissionProducer");

            migrationBuilder.RenameTable(
                name: "RegistrationSubmissionData",
                schema: "registration",
                newName: "RegistrationSubmissionData");
        }
    }
}
