using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingRequestorTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestorType",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestorType", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestorType",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 1, "Not Applicable", "NA" },
                    { 2, "Producers", "Producers" },
                    { 3, "Compliance Schemes", "ComplianceSchemes" },
                    { 4, "Exporters", "Exporters" },
                    { 5, "Reprocessors", "Reprocessors" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestorType",
                schema: "Lookup");
        }
    }
}
