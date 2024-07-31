using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegistrationFeesTablesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalRegistrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ComplianceSchemeRegistrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ProducerRegistrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "SubsidiariesRegistrationFees",
                schema: "Lookup");

            migrationBuilder.CreateTable(
                name: "Group",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regulator",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regulator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubGroup",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubGroupId = table.Column<int>(type: "int", nullable: false),
                    RegulatorId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFees_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Lookup",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationFees_Regulator_RegulatorId",
                        column: x => x.RegulatorId,
                        principalSchema: "Lookup",
                        principalTable: "Regulator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationFees_SubGroup_SubGroupId",
                        column: x => x.SubGroupId,
                        principalSchema: "Lookup",
                        principalTable: "SubGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Group",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 1, "Producer Type", "ProducerType" },
                    { 2, "Compliance Scheme", "ComplianceScheme" },
                    { 3, "Producer Subsidiaries", "ProducerSubsidiaries" },
                    { 4, "Compliance Scheme Subsidiaries", "ComplianceSchemeSubsidiaries" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Regulator",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 1, "England", "GB-ENG" },
                    { 2, "Scotland", "GB-SCT" },
                    { 3, "Wales", "GB-WLS" },
                    { 4, "Northern Ireland", "GB-NIR" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubGroup",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 1, "Large producer", "Large" },
                    { 2, "Small producer", "Small" },
                    { 3, "Registration", "Registration" },
                    { 4, "Online Market", "Online" },
                    { 5, "Up to 20", "UpTo20" },
                    { 6, "More than 20", "MoreThan20" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RegistrationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "GroupId", "RegulatorId", "SubGroupId" },
                values: new object[,]
                {
                    { 1, 2620m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, 2620m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 1 },
                    { 3, 2620m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 1 },
                    { 4, 2620m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 1 },
                    { 5, 1216m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 2 },
                    { 6, 1216m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 2 },
                    { 7, 1216m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 2 },
                    { 8, 1216m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 2 },
                    { 9, 1658m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1 },
                    { 10, 1658m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 1 },
                    { 11, 1658m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 1 },
                    { 12, 1658m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 1 },
                    { 13, 631m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 2 },
                    { 14, 631m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 2 },
                    { 15, 631m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 2 },
                    { 16, 631m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 2 },
                    { 17, 13804m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 3 },
                    { 18, 13804m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 3 },
                    { 19, 13804m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 3 },
                    { 20, 13804m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 3 },
                    { 21, 2579m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 4 },
                    { 22, 2579m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 4 },
                    { 23, 2579m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 4 },
                    { 24, 2579m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 4 },
                    { 25, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 5 },
                    { 26, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 5 },
                    { 27, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 5 },
                    { 28, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, 5 },
                    { 29, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 6 },
                    { 30, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 6 },
                    { 31, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 6 },
                    { 32, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4, 6 },
                    { 33, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 5 },
                    { 34, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, 5 },
                    { 35, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, 5 },
                    { 36, 558m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4, 5 },
                    { 37, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 6 },
                    { 38, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, 6 },
                    { 39, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, 6 },
                    { 40, 140m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFees_GroupId",
                schema: "Lookup",
                table: "RegistrationFees",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFees_RegulatorId",
                schema: "Lookup",
                table: "RegistrationFees",
                column: "RegulatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFees_SubGroupId",
                schema: "Lookup",
                table: "RegistrationFees",
                column: "SubGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Regulator",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "SubGroup",
                schema: "Lookup");

            migrationBuilder.CreateTable(
                name: "AdditionalRegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeesSubType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalRegistrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceSchemeRegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeesType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceSchemeRegistrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProducerRegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProducerType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerRegistrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubsidiariesRegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxNumberOfSubsidiaries = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    MinNumberOfSubsidiaries = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsidiariesRegistrationFees", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "AdditionalRegistrationFees",
                columns: new[] { "Id", "Amount", "Description", "EffectiveFrom", "EffectiveTo", "FeesSubType", "Regulator" },
                values: new object[,]
                {
                    { 1, 714m, "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub", "GB-ENG" },
                    { 2, 714m, "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub", "GB-SCT" },
                    { 3, 714m, "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub", "GB-WLS" },
                    { 4, 714m, "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub", "GB-NIR" },
                    { 5, 332m, "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late", "GB-ENG" },
                    { 6, 332m, "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late", "GB-SCT" },
                    { 7, 332m, "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late", "GB-WLS" },
                    { 8, 332m, "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late", "GB-NIR" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ComplianceSchemeRegistrationFees",
                columns: new[] { "Id", "Amount", "Description", "EffectiveFrom", "EffectiveTo", "FeesType", "Regulator" },
                values: new object[,]
                {
                    { 1, 13804m, "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg", "GB-ENG" },
                    { 2, 13804m, "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg", "GB-SCT" },
                    { 3, 13804m, "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg", "GB-WLS" },
                    { 4, 13804m, "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg", "GB-NIR" },
                    { 5, 1658m, "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-ENG" },
                    { 6, 1658m, "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-SCT" },
                    { 7, 1658m, "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-WLS" },
                    { 8, 1658m, "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-NIR" },
                    { 9, 631m, "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-ENG" },
                    { 10, 631m, "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-SCT" },
                    { 11, 631m, "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-WLS" },
                    { 12, 631m, "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-NIR" },
                    { 13, 2579m, "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On", "GB-ENG" },
                    { 14, 2579m, "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On", "GB-SCT" },
                    { 15, 2579m, "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On", "GB-WLS" },
                    { 16, 2579m, "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On", "GB-NIR" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ProducerRegistrationFees",
                columns: new[] { "Id", "Amount", "Description", "EffectiveFrom", "EffectiveTo", "ProducerType", "Regulator" },
                values: new object[,]
                {
                    { 1, 2620m, "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-ENG" },
                    { 2, 2620m, "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-SCT" },
                    { 3, 2620m, "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-WLS" },
                    { 4, 2620m, "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L", "GB-NIR" },
                    { 5, 1216m, "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-ENG" },
                    { 6, 1216m, "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-SCT" },
                    { 7, 1216m, "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-WLS" },
                    { 8, 1216m, "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S", "GB-NIR" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SubsidiariesRegistrationFees",
                columns: new[] { "Id", "Amount", "Description", "EffectiveFrom", "EffectiveTo", "MaxNumberOfSubsidiaries", "MinNumberOfSubsidiaries", "Regulator" },
                values: new object[,]
                {
                    { 1, 558m, "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, "GB-ENG" },
                    { 2, 558m, "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, "GB-SCT" },
                    { 3, 558m, "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, "GB-WLS" },
                    { 4, 558m, "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, "GB-NIR" },
                    { 5, 140m, "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21, "GB-ENG" },
                    { 6, 140m, "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21, "GB-SCT" },
                    { 7, 140m, "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21, "GB-WLS" },
                    { 8, 140m, "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21, "GB-NIR" }
                });
        }
    }
}
