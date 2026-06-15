using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUpdatedDateFromRegistrationSubmissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "registration",
                table: "RegistrationSubmissionData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                schema: "registration",
                table: "RegistrationSubmissionData",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
