using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationSubmissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "registration");

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionData",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationBlobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComplianceSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmissionPeriod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSubmissionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionProducer",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationSubmissionDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrganisationSize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false),
                    IsOnlineMarketplace = table.Column<bool>(type: "bit", nullable: false),
                    IsClosedLoopRecycling = table.Column<bool>(type: "bit", nullable: false),
                    IsNewJoiner = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSubmissionProducer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationSubmissionProducer_RegistrationSubmissionData_RegistrationSubmissionDataId",
                        column: x => x.RegistrationSubmissionDataId,
                        principalSchema: "registration",
                        principalTable: "RegistrationSubmissionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionSubsidiary",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationSubmissionProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubsidiaryId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsOnlineMarketplace = table.Column<bool>(type: "bit", nullable: false),
                    IsClosedLoopRecycling = table.Column<bool>(type: "bit", nullable: false),
                    IsNewJoiner = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSubmissionSubsidiary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationSubmissionSubsidiary_RegistrationSubmissionProducer_RegistrationSubmissionProducerId",
                        column: x => x.RegistrationSubmissionProducerId,
                        principalSchema: "registration",
                        principalTable: "RegistrationSubmissionProducer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionData_RegistrationBlobName",
                schema: "registration",
                table: "RegistrationSubmissionData",
                column: "RegistrationBlobName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionProducer_RegistrationSubmissionDataId",
                schema: "registration",
                table: "RegistrationSubmissionProducer",
                column: "RegistrationSubmissionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionSubsidiary_RegistrationSubmissionProducerId",
                schema: "registration",
                table: "RegistrationSubmissionSubsidiary",
                column: "RegistrationSubmissionProducerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationSubmissionSubsidiary",
                schema: "registration");

            migrationBuilder.DropTable(
                name: "RegistrationSubmissionProducer",
                schema: "registration");

            migrationBuilder.DropTable(
                name: "RegistrationSubmissionData",
                schema: "registration");
        }
    }
}
