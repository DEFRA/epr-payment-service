using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationSubmissionDataEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionDataEvents",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationSubmissionDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSubmissionDataEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationSubmissionDataEvents_RegistrationSubmissionData_RegistrationSubmissionDataId",
                        column: x => x.RegistrationSubmissionDataId,
                        principalSchema: "registration",
                        principalTable: "RegistrationSubmissionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionDataEvents_SubmissionData_Event_Date_Unique",
                schema: "registration",
                table: "RegistrationSubmissionDataEvents",
                columns: new[] { "RegistrationSubmissionDataId", "EventName", "EventDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationSubmissionDataEvents",
                schema: "registration");
        }
    }
}
