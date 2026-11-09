using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SUB365addregistrationhistorymigrationschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "migration");

            migrationBuilder.CreateTable(
                name: "MigrationRun",
                schema: "migration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RunKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    InstanceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StagingCompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    StagedRowCount = table.Column<int>(type: "int", nullable: false),
                    SubmissionCount = table.Column<int>(type: "int", nullable: false),
                    SucceededCount = table.Column<int>(type: "int", nullable: false),
                    FailedCount = table.Column<int>(type: "int", nullable: false),
                    NotRequiredCount = table.Column<int>(type: "int", nullable: false),
                    ClearDownApplied = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigrationRun", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MigratedEntity",
                schema: "migration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistrationSubmissionDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigratedEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MigratedEntity_MigrationRun_RunId",
                        column: x => x.RunId,
                        principalSchema: "migration",
                        principalTable: "MigrationRun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationHistoryStaging",
                schema: "migration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BlobName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrganisationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ComplianceSchemeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmissionPeriod = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RegistrationJourney = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RegulatorNation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventTypeOrder = table.Column<int>(type: "int", nullable: false),
                    ReplayTs = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationReferenceNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Decision = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationHistoryStaging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationHistoryStaging_MigrationRun_RunId",
                        column: x => x.RunId,
                        principalSchema: "migration",
                        principalTable: "MigrationRun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationSubmissionMigration",
                schema: "migration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warnings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilesMigrated = table.Column<int>(type: "int", nullable: false),
                    EventsReplayed = table.Column<int>(type: "int", nullable: false),
                    EventsSkipped = table.Column<int>(type: "int", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSubmissionMigration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationSubmissionMigration_MigrationRun_RunId",
                        column: x => x.RunId,
                        principalSchema: "migration",
                        principalTable: "MigrationRun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MigratedEntity_RunId_RegistrationSubmissionDataId",
                schema: "migration",
                table: "MigratedEntity",
                columns: new[] { "RunId", "RegistrationSubmissionDataId" });

            migrationBuilder.CreateIndex(
                name: "IX_MigrationRun_RunKey",
                schema: "migration",
                table: "MigrationRun",
                column: "RunKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistoryStaging_RunId_SubmissionId_ReplayTs",
                schema: "migration",
                table: "RegistrationHistoryStaging",
                columns: new[] { "RunId", "SubmissionId", "ReplayTs" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionMigration_RunId_Status",
                schema: "migration",
                table: "RegistrationSubmissionMigration",
                columns: new[] { "RunId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSubmissionMigration_RunId_SubmissionId",
                schema: "migration",
                table: "RegistrationSubmissionMigration",
                columns: new[] { "RunId", "SubmissionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MigratedEntity",
                schema: "migration");

            migrationBuilder.DropTable(
                name: "RegistrationHistoryStaging",
                schema: "migration");

            migrationBuilder.DropTable(
                name: "RegistrationSubmissionMigration",
                schema: "migration");

            migrationBuilder.DropTable(
                name: "MigrationRun",
                schema: "migration");
        }
    }
}
