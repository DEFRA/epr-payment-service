using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SUB365addstagingunselectedappsubmitted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EarliestUnselectedAppSubmittedDate",
                schema: "migration",
                table: "RegistrationHistoryStaging",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnselectedAppSubmittedFileCount",
                schema: "migration",
                table: "RegistrationHistoryStaging",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarliestUnselectedAppSubmittedDate",
                schema: "migration",
                table: "RegistrationHistoryStaging");

            migrationBuilder.DropColumn(
                name: "UnselectedAppSubmittedFileCount",
                schema: "migration",
                table: "RegistrationHistoryStaging");
        }
    }
}
