using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationSubmissionProducerAndSubsidiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "RegistrationSubmissionData");

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionProducer",
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
                        principalTable: "RegistrationSubmissionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionSubsidiary",
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
                        principalTable: "RegistrationSubmissionProducer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionProducer_RegistrationSubmissionDataId",
                table: "RegistrationSubmissionProducer",
                column: "RegistrationSubmissionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionSubsidiary_RegistrationSubmissionProducerId",
                table: "RegistrationSubmissionSubsidiary",
                column: "RegistrationSubmissionProducerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationSubmissionSubsidiary");

            migrationBuilder.DropTable(
                name: "RegistrationSubmissionProducer");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "RegistrationSubmissionData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
