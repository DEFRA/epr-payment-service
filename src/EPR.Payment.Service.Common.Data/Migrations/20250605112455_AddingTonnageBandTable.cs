using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingTonnageBandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TonnageBand",
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
                    table.PrimaryKey("PK_TonnageBand", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "TonnageBand",
                columns: new[] { "Id", "Description", "Type" },
                values: new object[,]
                {
                    { 1, "Tonnage upto 500 tonnes", "Upto500" },
                    { 2, "Tonnage over 500 to 5000 tonnes", "Over500To5000" },
                    { 3, "Tonnage over 5000 to 10000 tonnes", "Over5000To10000" },
                    { 4, "Tonnage over 10000 tonnes", "Over10000" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TonnageBand",
                schema: "Lookup");
        }
    }
}
