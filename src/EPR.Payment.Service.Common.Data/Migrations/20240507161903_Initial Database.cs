using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.CreateTable(
                name: "AccreditationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Large = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalPaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovpayPaymentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InternalStatusId = table.Column<int>(type: "int", nullable: false),
                    Regulator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GovPayStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
                        name: "FK_Payment_PaymentStatus_InternalStatusId",
                        column: x => x.InternalStatusId,
                        principalSchema: "Lookup",
                        principalTable: "PaymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "AccreditationFees",
                columns: new[] { "Id", "Amount", "EffectiveFrom", "EffectiveTo", "Large", "Regulator" },
                values: new object[,]
                {
                    { 1, 2616m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "GB-ENG" },
                    { 2, 2616m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "GB-SCT" },
                    { 3, 2616m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "GB-WLS" },
                    { 4, 2616m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "GB-NIR" },
                    { 5, 505m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "GB-ENG" },
                    { 6, 505m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "GB-SCT" },
                    { 7, 505m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "GB-WLS" },
                    { 8, 505m, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "GB-NIR" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "PaymentStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 0, "Initiated" },
                    { 1, "Success" },
                    { 2, "Failed" },
                    { 3, "Error" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GovpayPaymentId",
                table: "Payment",
                column: "GovpayPaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InternalStatusId",
                table: "Payment",
                column: "InternalStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccreditationFees",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentStatus",
                schema: "Lookup");
        }
    }
}
