using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAccreditationFees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccreditationFees",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubGroupId = table.Column<int>(type: "int", nullable: false),
                    RegulatorId = table.Column<int>(type: "int", nullable: false),
                    TonnesUpTo = table.Column<int>(type: "int", nullable: false),
                    TonnesOver = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    FeePerSite = table.Column<int>(type: "int", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditationFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccreditationFees_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Lookup",
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccreditationFees_Regulator_RegulatorId",
                        column: x => x.RegulatorId,
                        principalSchema: "Lookup",
                        principalTable: "Regulator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccreditationFees_SubGroup_SubGroupId",
                        column: x => x.SubGroupId,
                        principalSchema: "Lookup",
                        principalTable: "SubGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationFees_GroupId",
                schema: "Lookup",
                table: "AccreditationFees",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationFees_RegulatorId",
                schema: "Lookup",
                table: "AccreditationFees",
                column: "RegulatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationFees_SubGroupId",
                schema: "Lookup",
                table: "AccreditationFees",
                column: "SubGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccreditationFees",
                schema: "Lookup");
        }
    }
}
