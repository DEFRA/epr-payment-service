using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTableExternalPaymentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_InternalError_InternalErrorCode",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "InternalError",
                schema: "Lookup");

            migrationBuilder.DropIndex(
                name: "IX_Payment_InternalErrorCode",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "InternalErrorCode",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Lookup",
                table: "PaymentStatus",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Regulator",
                table: "Payment",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ErrorCode",
                table: "Payment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "Payment",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalPaymentId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.Sql(@"
                Update Payment set ExternalPaymentId = NEWID() 
                where ExternalPaymentId = '00000000-0000-0000-0000-000000000000'");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ExternalPaymentId",
                table: "Payment",
                column: "ExternalPaymentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_ExternalPaymentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ErrorCode",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ExternalPaymentId",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Lookup",
                table: "PaymentStatus",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Regulator",
                table: "Payment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "InternalErrorCode",
                table: "Payment",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InternalError",
                schema: "Lookup",
                columns: table => new
                {
                    InternalErrorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GovPayErrorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GovPayErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalError", x => x.InternalErrorCode);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "InternalError",
                columns: new[] { "InternalErrorCode", "ErrorMessage", "GovPayErrorCode", "GovPayErrorMessage" },
                values: new object[,]
                {
                    { "A", null, "P0030", "Cancelled" },
                    { "B", null, "P0020", "Expired" },
                    { "C", null, "P0010", "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InternalErrorCode",
                table: "Payment",
                column: "InternalErrorCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_InternalError_InternalErrorCode",
                table: "Payment",
                column: "InternalErrorCode",
                principalSchema: "Lookup",
                principalTable: "InternalError",
                principalColumn: "InternalErrorCode");
        }
    }
}
