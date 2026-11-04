using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPR.Payment.Service.Common.Data.Migrations
{
    /// <summary>
    /// Creates the [registration_pre_migration] schema the registration history migration backs [registration] up to
    /// before clearing it. Created here, by the pipeline, because the service's login cannot create schemas or tables;
    /// at run time the service only empties and refills these tables (Scripts/Migration/backup-registration-pre-migration.sql).
    /// Not part of the EF model. Tables mirror [registration] (same columns and types, no keys or indexes);
    /// payments_unlinked records the payments the clear-down unlinks.
    /// </summary>
    public partial class SUB365addregistrationpremigrationschema : Migration
    {
        private const string Schema = "registration_pre_migration";

        private static readonly string[] RegistrationTables =
        {
            "RegistrationSubmissionData",
            "RegistrationSubmissionProducer",
            "RegistrationSubmissionSubsidiary",
            "RegistrationSubmissionDataEvents",
            "RegistrationFeeSnapshot",
            "RegistrationFeeLineItem",
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(Schema);

            // Guarded so a schema created by hand earlier (the manual backup script) does not fail the migration.
            foreach (var table in RegistrationTables)
            {
                migrationBuilder.Sql(
                    $"IF OBJECT_ID(N'[{Schema}].[{table}]') IS NULL SELECT TOP (0) * INTO [{Schema}].[{table}] FROM [registration].[{table}];");
            }

            migrationBuilder.Sql(
                $@"IF OBJECT_ID(N'[{Schema}].[payments_unlinked]') IS NULL
CREATE TABLE [{Schema}].[payments_unlinked] (
    [PaymentId] int NOT NULL,
    [ExternalPaymentId] uniqueidentifier NULL,
    [Reference] nvarchar(255) NULL,
    [Amount] decimal(19,4) NULL,
    [ReasonForPayment] nvarchar(255) NULL,
    [Regulator] varchar(20) NULL,
    [InternalStatusId] int NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    [RegistrationSubmissionDataId] uniqueidentifier NULL,
    [BackedUpAt] datetime2 NOT NULL
);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP TABLE IF EXISTS [{Schema}].[payments_unlinked];");

            foreach (var table in RegistrationTables)
            {
                migrationBuilder.Sql($"DROP TABLE IF EXISTS [{Schema}].[{table}];");
            }

            migrationBuilder.Sql($"IF SCHEMA_ID(N'{Schema}') IS NOT NULL DROP SCHEMA [{Schema}];");
        }
    }
}
