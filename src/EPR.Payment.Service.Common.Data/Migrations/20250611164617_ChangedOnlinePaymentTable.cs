using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnlinePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestorType",
                table: "OnlinePayment");

            migrationBuilder.AddColumn<int>(
                name: "RequestorTypeId",
                table: "OnlinePayment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePayment_RequestorTypeId",
                table: "OnlinePayment",
                column: "RequestorTypeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlinePayment_RequestorType_RequestorTypeId",
                table: "OnlinePayment",
                column: "RequestorTypeId",
                principalSchema: "Lookup",
                principalTable: "RequestorType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlinePayment_RequestorType_RequestorTypeId",
                table: "OnlinePayment");

            migrationBuilder.DropIndex(
                name: "IX_OnlinePayment_RequestorTypeId",
                table: "OnlinePayment");

            migrationBuilder.DropColumn(
                name: "RequestorTypeId",
                table: "OnlinePayment");

            migrationBuilder.AddColumn<string>(
                name: "RequestorType",
                table: "OnlinePayment",
                type: "nvarchar(50)",
                nullable: true);
        }
    }
}
