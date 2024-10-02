using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class LateFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Type" },
                values: new object[] { "Late Fee", "LateFee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "SubGroup",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Type" },
                values: new object[] { "Producer Late Registration Fee", "ProducerLateRegistrationFee" });
        }
    }
}
