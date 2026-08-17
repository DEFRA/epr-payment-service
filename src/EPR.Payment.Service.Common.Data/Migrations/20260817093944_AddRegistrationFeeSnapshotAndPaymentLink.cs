using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationFeeSnapshotAndPaymentLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationSubmissionDataId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 12);

            migrationBuilder.CreateTable(
                name: "RegistrationFeeSnapshot",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationSubmissionDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalFee = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFeeSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFeeSnapshot_RegistrationSubmissionData_RegistrationSubmissionDataId",
                        column: x => x.RegistrationSubmissionDataId,
                        principalSchema: "registration",
                        principalTable: "RegistrationSubmissionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationFeeLineItem",
                schema: "registration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RegistrationFeeSnapshotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeTypeId = table.Column<int>(type: "int", nullable: false),
                    FeeTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BandNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFeeLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationFeeLineItem_RegistrationFeeSnapshot_RegistrationFeeSnapshotId",
                        column: x => x.RegistrationFeeSnapshotId,
                        principalSchema: "registration",
                        principalTable: "RegistrationFeeSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RegistrationSubmissionDataId",
                table: "Payment",
                column: "RegistrationSubmissionDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFeeLineItem_RegistrationFeeSnapshotId_FeeTypeId",
                schema: "registration",
                table: "RegistrationFeeLineItem",
                columns: new[] { "RegistrationFeeSnapshotId", "FeeTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationFeeSnapshot_RegistrationSubmissionDataId",
                schema: "registration",
                table: "RegistrationFeeSnapshot",
                column: "RegistrationSubmissionDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_RegistrationSubmissionData_RegistrationSubmissionDataId",
                table: "Payment",
                column: "RegistrationSubmissionDataId",
                principalSchema: "registration",
                principalTable: "RegistrationSubmissionData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_RegistrationSubmissionData_RegistrationSubmissionDataId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "RegistrationFeeLineItem",
                schema: "registration");

            migrationBuilder.DropTable(
                name: "RegistrationFeeSnapshot",
                schema: "registration");

            migrationBuilder.DropIndex(
                name: "IX_Payment_RegistrationSubmissionDataId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "RegistrationSubmissionDataId",
                table: "Payment");
        }
    }
}
