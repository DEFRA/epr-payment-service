using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Lookup");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentStatus",
                schema: "Lookup");
        }
    }
}
