using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccreditationFeeTableDesignChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TonnesOver",
                schema: "Lookup",
                table: "AccreditationFees");

            migrationBuilder.DropColumn(
                name: "TonnesUpTo",
                schema: "Lookup",
                table: "AccreditationFees");

            migrationBuilder.AddColumn<int>(
                name: "TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 1,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 2,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 3,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 4,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 5,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 6,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 7,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 8,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 9,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 10,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 11,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 12,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 13,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 14,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 15,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 16,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 17,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 18,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 19,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 20,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 21,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 22,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 23,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 24,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 25,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 26,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 27,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 28,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 29,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 30,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 31,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 32,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 33,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 34,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 35,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 36,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 37,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 38,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 39,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 40,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 41,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 42,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 43,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 44,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 45,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 46,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 47,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 48,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 49,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 50,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 51,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 52,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 53,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 54,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 55,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 56,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 57,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 58,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 59,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 60,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 61,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 62,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 63,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 64,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 65,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 66,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 67,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 68,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 69,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 70,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 71,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 72,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 73,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 74,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 75,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 76,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 77,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 78,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 79,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 80,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 81,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 82,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 83,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 84,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 85,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 86,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 87,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 88,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 89,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 90,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 91,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 92,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 93,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 94,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 95,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 96,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 97,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 98,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 99,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 100,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 101,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 102,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 103,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 104,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 105,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 106,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 107,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 108,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 109,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 110,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 111,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 112,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 113,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 114,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 115,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 116,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 117,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 118,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 119,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 120,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 121,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 122,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 123,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 124,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 125,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 126,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 127,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 128,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 129,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 130,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 131,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 132,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 133,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 134,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 135,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 136,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 137,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 138,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 139,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 140,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 141,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 142,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 143,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 144,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 145,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 146,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 147,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 148,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 149,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 150,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 151,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 152,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 153,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 154,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 155,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 156,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 157,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 158,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 159,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 160,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 161,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 162,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 163,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 164,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 165,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 166,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 167,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 168,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 169,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 170,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 171,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 172,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 173,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 174,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 175,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 176,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 177,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 178,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 179,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 180,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 181,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 182,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 183,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 184,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 185,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 186,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 187,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 188,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 189,
                column: "TonnageBandId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 190,
                column: "TonnageBandId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 191,
                column: "TonnageBandId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 192,
                column: "TonnageBandId",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_AccreditationFees_TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees",
                column: "TonnageBandId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccreditationFees_TonnageBand_TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees",
                column: "TonnageBandId",
                principalSchema: "Lookup",
                principalTable: "TonnageBand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccreditationFees_TonnageBand_TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees");

            migrationBuilder.DropIndex(
                name: "IX_AccreditationFees_TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees");

            migrationBuilder.DropColumn(
                name: "TonnageBandId",
                schema: "Lookup",
                table: "AccreditationFees");

            migrationBuilder.AddColumn<int>(
                name: "TonnesOver",
                schema: "Lookup",
                table: "AccreditationFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TonnesUpTo",
                schema: "Lookup",
                table: "AccreditationFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 0, 500 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 500, 5000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 5000, 10000 });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "AccreditationFees",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "TonnesOver", "TonnesUpTo" },
                values: new object[] { 10000, 99999999 });
        }
    }
}
