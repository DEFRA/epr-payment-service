using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class SUB262extend2026feesenddate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [Lookup].[RegistrationFees]
   SET [EffectiveTo] = '2050-12-31 23:59:59'
 WHERE [EffectiveFrom] = '2026-01-01 00:00:00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [Lookup].[RegistrationFees]
   SET [EffectiveTo] = '2026-12-31 23:59:59'
 WHERE [EffectiveFrom] = '2026-01-01 00:00:00';");
        }
    }
}
