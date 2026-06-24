using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterSubmissionPeriodMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionPeriod",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionPeriod",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
