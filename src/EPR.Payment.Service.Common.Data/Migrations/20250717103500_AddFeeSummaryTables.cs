using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeeSummaryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayerTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppRefNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoicePeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayerTypeId = table.Column<int>(type: "int", nullable: false),
                    PayerId = table.Column<int>(type: "int", nullable: false),
                    FeeTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeeTypeId1 = table.Column<int>(type: "int", nullable: true),
                    PayerTypeId1 = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_FeeSummaries_FeeTypes_FeeTypeId1",
                        column: x => x.FeeTypeId1,
                        principalSchema: "Lookup",
                        principalTable: "FeeTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeeSummaries_PayerTypes_PayerTypeId",
                        column: x => x.PayerTypeId,
                        principalSchema: "Lookup",
                        principalTable: "PayerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeSummaries_PayerTypes_PayerTypeId1",
                        column: x => x.PayerTypeId1,
                        principalSchema: "Lookup",
                        principalTable: "PayerTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileFeeSummaryConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeSummaryId = table.Column<int>(type: "int", nullable: false),
                    FeeSummaryId1 = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_FileFeeSummaryConnections_FeeSummaries_FeeSummaryId1",
                        column: x => x.FeeSummaryId1,
                        principalTable: "FeeSummaries",
                        principalColumn: "Id");
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
                    { 8, "Late Registration Fee" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "PayerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Direct Producer" },
                    { 2, "Compliance Scheme" },
                    { 3, "Reprocessor" },
                    { 4, "Exporter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_FeeTypeId",
                table: "FeeSummaries",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_FeeTypeId1",
                table: "FeeSummaries",
                column: "FeeTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_PayerTypeId",
                table: "FeeSummaries",
                column: "PayerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSummaries_PayerTypeId1",
                table: "FeeSummaries",
                column: "PayerTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_FileFeeSummaryConnections_FeeSummaryId",
                table: "FileFeeSummaryConnections",
                column: "FeeSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_FileFeeSummaryConnections_FeeSummaryId1",
                table: "FileFeeSummaryConnections",
                column: "FeeSummaryId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileFeeSummaryConnections");

            migrationBuilder.DropTable(
                name: "FeeSummaries");

            migrationBuilder.DropTable(
                name: "FeeTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "PayerTypes",
                schema: "Lookup");
        }
    }
}
