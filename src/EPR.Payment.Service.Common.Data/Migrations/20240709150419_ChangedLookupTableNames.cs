using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLookupTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ComplianceShemeRegitrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ProducerRegitrationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Subsidiaries",
                schema: "Lookup");

            migrationBuilder.CreateTable(
                name: "AdditionalRegistrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesSubType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    FeesType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    ProducerType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    MinNumberOfSubsidiaries = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    MaxNumberOfSubsidiaries = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "AdditionalFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeesSubType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceShemeRegitrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeesType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceShemeRegitrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProducerRegitrationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProducerType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerRegitrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subsidiaries",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxSub = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    MinSub = table.Column<int>(type: "int", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidiaries", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "AdditionalFees",
                columns: new[] { "Id", "Amount", "Country", "Description", "EffectiveFrom", "EffectiveTo", "FeesSubType" },
                values: new object[,]
                {
                    { 1, 714m, "GB-ENG", "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub" },
                    { 2, 714m, "GB-SCT", "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub" },
                    { 3, 714m, "GB-WLS", "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub" },
                    { 4, 714m, "GB-NIR", "Resubmission", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resub" },
                    { 5, 332m, "GB-ENG", "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late" },
                    { 6, 332m, "GB-SCT", "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late" },
                    { 7, 332m, "GB-WLS", "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late" },
                    { 8, 332m, "GB-NIR", "Late", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ComplianceShemeRegitrationFees",
                columns: new[] { "Id", "Amount", "Country", "Description", "EffectiveFrom", "EffectiveTo", "FeesType" },
                values: new object[,]
                {
                    { 1, 13804m, "GB-ENG", "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg" },
                    { 2, 13804m, "GB-SCT", "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg" },
                    { 3, 13804m, "GB-WLS", "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg" },
                    { 4, 13804m, "GB-NIR", "Registration", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reg" },
                    { 5, 1658m, "GB-ENG", "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 6, 1658m, "GB-SCT", "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 7, 1658m, "GB-WLS", "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 8, 1658m, "GB-NIR", "Large Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 9, 631m, "GB-ENG", "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 10, 631m, "GB-SCT", "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 11, 631m, "GB-WLS", "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 12, 631m, "GB-NIR", "Small Producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 13, 2579m, "GB-ENG", "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On" },
                    { 14, 2579m, "GB-SCT", "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On" },
                    { 15, 2579m, "GB-WLS", "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On" },
                    { 16, 2579m, "GB-NIR", "Online Market", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ProducerRegitrationFees",
                columns: new[] { "Id", "Amount", "Country", "Description", "EffectiveFrom", "EffectiveTo", "ProducerType" },
                values: new object[,]
                {
                    { 1, 2620m, "GB-ENG", "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 2, 2620m, "GB-SCT", "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 3, 2620m, "GB-WLS", "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 4, 2620m, "GB-NIR", "Large producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L" },
                    { 5, 1216m, "GB-ENG", "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 6, 1216m, "GB-SCT", "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 7, 1216m, "GB-WLS", "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" },
                    { 8, 1216m, "GB-NIR", "Small producer", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "S" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Subsidiaries",
                columns: new[] { "Id", "Amount", "Country", "Description", "EffectiveFrom", "EffectiveTo", "MaxSub", "MinSub" },
                values: new object[,]
                {
                    { 1, 558m, "GB-ENG", "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1 },
                    { 2, 558m, "GB-SCT", "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1 },
                    { 3, 558m, "GB-WLS", "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1 },
                    { 4, 558m, "GB-NIR", "Up to 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1 },
                    { 5, 140m, "GB-ENG", "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21 },
                    { 6, 140m, "GB-SCT", "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21 },
                    { 7, 140m, "GB-WLS", "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21 },
                    { 8, 140m, "GB-NIR", "More then 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 21 }
                });
        }
    }
}
