using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOfflinePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "OfflinePayment");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "OfflinePayment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_OfflinePayment_PaymentMethodId",
                table: "OfflinePayment",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflinePayment_PaymentMethod_PaymentMethodId",
                table: "OfflinePayment",
                column: "PaymentMethodId",
                principalSchema: "Lookup",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflinePayment_PaymentMethod_PaymentMethodId",
                table: "OfflinePayment");

            migrationBuilder.DropIndex(
                name: "IX_OfflinePayment_PaymentMethodId",
                table: "OfflinePayment");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "OfflinePayment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "OfflinePayment",
                type: "nvarchar(20)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);
        }
    }
}
