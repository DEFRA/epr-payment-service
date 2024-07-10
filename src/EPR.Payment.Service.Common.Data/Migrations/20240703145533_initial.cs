using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.CreateTable(
                name: "AdditionalFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesSubType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceShemeRegitrationFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceShemeRegitrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalError",
                columns: table => new
                {
                    InternalErrorCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GovPayErrorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GovPayErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalError", x => x.InternalErrorCode);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProducerRegitrationFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProducerType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerRegitrationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subsidiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinSub = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    MaxSub = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidiaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovpayPaymentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InternalStatusId = table.Column<int>(type: "int", nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GovPayStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InternalErrorCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    ReasonForPayment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedByOrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_InternalError_InternalErrorCode",
                        column: x => x.InternalErrorCode,
                        principalTable: "InternalError",
                        principalColumn: "InternalErrorCode");
                    table.ForeignKey(
                        name: "FK_Payment_PaymentStatus_InternalStatusId",
                        column: x => x.InternalStatusId,
                        principalSchema: "Lookup",
                        principalTable: "PaymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
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
                table: "InternalError",
                columns: new[] { "InternalErrorCode", "ErrorMessage", "GovPayErrorCode", "GovPayErrorMessage" },
                values: new object[,]
                {
                    { "A", null, "P0030", "Cancelled" },
                    { "B", null, "P0020", "Expired" },
                    { "C", null, "P0010", "Rejected" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "PaymentStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 0, "Initiated" },
                    { 1, "InProgress" },
                    { 2, "Success" },
                    { 3, "Failed" },
                    { 4, "Error" },
                    { 5, "UserCancelled" }
                });

            migrationBuilder.InsertData(
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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GovpayPaymentId",
                table: "Payment",
                column: "GovpayPaymentId",
                unique: true,
                filter: "[GovpayPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InternalErrorCode",
                table: "Payment",
                column: "InternalErrorCode");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InternalStatusId",
                table: "Payment",
                column: "InternalStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalFees");

            migrationBuilder.DropTable(
                name: "ComplianceShemeRegitrationFees");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ProducerRegitrationFees");

            migrationBuilder.DropTable(
                name: "Subsidiaries");

            migrationBuilder.DropTable(
                name: "InternalError");

            migrationBuilder.DropTable(
                name: "PaymentStatus",
                schema: "Lookup");
        }
    }
}
