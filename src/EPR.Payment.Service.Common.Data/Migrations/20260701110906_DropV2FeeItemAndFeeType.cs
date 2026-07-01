using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropV2FeeItemAndFeeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeItem");

            migrationBuilder.DropTable(
                name: "FeeTypes",
                schema: "Lookup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeTypeId = table.Column<int>(type: "int", nullable: false),
                    PayerTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AppRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InvoicePeriod = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PayerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeItem_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalSchema: "Lookup",
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeItem_PayerTypes_PayerTypeId",
                        column: x => x.PayerTypeId,
                        principalSchema: "Lookup",
                        principalTable: "PayerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "FeeTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Producer Registration Fee" },
                    { 2, "Compliance Scheme Registration Fee" },
                    { 3, "Producer OnlineMarketPlace Fee" },
                    { 4, "Member Registration Fee" },
                    { 5, "Member Late Registration Fee" },
                    { 6, "UnitOMP Fee" },
                    { 7, "Subsidiary Fee" },
                    { 8, "Late Registration Fee" },
                    { 9, "Producer Resubmission Fee" },
                    { 10, "Compliance Scheme Resubmission" },
                    { 11, "FeePreviousPayment" },
                    { 12, "OutstandingPayment" },
                    { 13, "BandNumber 1" },
                    { 14, "BandNumber 2" },
                    { 15, "BandNumber 3" },
                    { 16, "PreviousPayment(reuse)" },
                    { 17, "OutstandingPayment(reuse)" },
                    { 18, "Member OnlineMarketPlace Fee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeItem_FeeTypeId",
                table: "FeeItem",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeItem_PayerTypeId",
                table: "FeeItem",
                column: "PayerTypeId");
        }
    }
}
