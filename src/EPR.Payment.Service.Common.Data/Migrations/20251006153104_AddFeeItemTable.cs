using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeeItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileFeeSummaryConnections");

            migrationBuilder.DropTable(
                name: "FeeSummaries");

            migrationBuilder.CreateTable(
                name: "FeeItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InvoicePeriod = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PayerTypeId = table.Column<int>(type: "int", nullable: false),
                    PayerId = table.Column<int>(type: "int", nullable: false),
                    FeeTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    { 9, "Producer Resubmission Fee" },
                    { 10, "Compliance Scheme Resubmission Fee" }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeItem");

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "FeeTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.CreateTable(
                name: "FeeSummaries",
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
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InvoicePeriod = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PayerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeSummaries_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalSchema: "Lookup",
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeSummaries_PayerTypes_PayerTypeId",
                        column: x => x.PayerTypeId,
                        principalSchema: "Lookup",
                        principalTable: "PayerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileFeeSummaryConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeSummaryId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFeeSummaryConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileFeeSummaryConnections_FeeSummaries_FeeSummaryId",
                        column: x => x.FeeSummaryId,
                        principalTable: "FeeSummaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_FeeTypeId",
                table: "FeeSummaries",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_PayerTypeId",
                table: "FeeSummaries",
                column: "PayerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileFeeSummaryConnections_FeeSummaryId",
                table: "FileFeeSummaryConnections",
                column: "FeeSummaryId");
        }
    }
}
