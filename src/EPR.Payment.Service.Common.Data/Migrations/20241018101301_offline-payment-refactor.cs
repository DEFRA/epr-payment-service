using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class offlinepaymentrefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Payment]", true);

            migrationBuilder.DropIndex(
                name: "IX_Payment_GovpayPaymentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ErrorCode",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "GovPayStatus",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "GovpayPaymentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Payment");

            migrationBuilder.CreateTable(
                name: "OnlinePayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovpayPaymentId = table.Column<string>(type: "varchar(50)", nullable: true),
                    GovPayStatus = table.Column<string>(type: "varchar(20)", nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(255)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlinePayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlinePayment_Payment_Id",
                        column: x => x.Id,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePayment_GovpayPaymentId",
                table: "OnlinePayment",
                column: "GovpayPaymentId",
                unique: true,
                filter: "[GovpayPaymentId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlinePayment");

            migrationBuilder.AddColumn<string>(
                name: "ErrorCode",
                table: "Payment",
                type: "varchar(255)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "Payment",
                type: "varchar(255)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.AddColumn<string>(
                name: "GovPayStatus",
                table: "Payment",
                type: "varchar(20)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<string>(
                name: "GovpayPaymentId",
                table: "Payment",
                type: "varchar(50)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GovpayPaymentId",
                table: "Payment",
                column: "GovpayPaymentId",
                unique: true,
                filter: "[GovpayPaymentId] IS NOT NULL");
        }
    }
}
