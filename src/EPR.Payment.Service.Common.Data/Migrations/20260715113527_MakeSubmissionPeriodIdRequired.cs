using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeSubmissionPeriodIdRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Purge any rows written since 20260709105007 with a null SubmissionPeriodId.
            // Cascade FKs on Producer/Subsidiary handle the child rows automatically.
            migrationBuilder.Sql(
                "DELETE FROM [registration].[RegistrationSubmissionData] WHERE [SubmissionPeriodId] IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SubmissionPeriodId",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
