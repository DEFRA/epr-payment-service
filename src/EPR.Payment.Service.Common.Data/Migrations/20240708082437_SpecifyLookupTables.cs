using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SpecifyLookupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Subsidiaries",
                newName: "Subsidiaries",
                newSchema: "Lookup");

            migrationBuilder.RenameTable(
                name: "ProducerRegitrationFees",
                newName: "ProducerRegitrationFees",
                newSchema: "Lookup");

            migrationBuilder.RenameTable(
                name: "InternalError",
                newName: "InternalError",
                newSchema: "Lookup");

            migrationBuilder.RenameTable(
                name: "ComplianceShemeRegitrationFees",
                newName: "ComplianceShemeRegitrationFees",
                newSchema: "Lookup");

            migrationBuilder.RenameTable(
                name: "AdditionalFees",
                newName: "AdditionalFees",
                newSchema: "Lookup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Subsidiaries",
                schema: "Lookup",
                newName: "Subsidiaries");

            migrationBuilder.RenameTable(
                name: "ProducerRegitrationFees",
                schema: "Lookup",
                newName: "ProducerRegitrationFees");

            migrationBuilder.RenameTable(
                name: "InternalError",
                schema: "Lookup",
                newName: "InternalError");

            migrationBuilder.RenameTable(
                name: "ComplianceShemeRegitrationFees",
                schema: "Lookup",
                newName: "ComplianceShemeRegitrationFees");

            migrationBuilder.RenameTable(
                name: "AdditionalFees",
                schema: "Lookup",
                newName: "AdditionalFees");
        }
    }
}
