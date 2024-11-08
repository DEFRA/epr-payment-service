using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class CSUpdateLargeFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 168500m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 168500m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 168500m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "Amount",
                value: 168500m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "Amount",
                value: 165800m);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "RegistrationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "Amount",
                value: 165800m);
        }
    }
}
