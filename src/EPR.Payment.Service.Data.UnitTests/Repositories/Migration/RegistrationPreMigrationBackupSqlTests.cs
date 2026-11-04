using EPR.Payment.Service.Common.Data.Repositories.Migration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Migration
{
    [TestClass]
    public class RegistrationPreMigrationBackupSqlTests
    {
        [TestMethod]
        public void Load_ReturnsEmbeddedBackupScript()
        {
            var sql = RegistrationPreMigrationBackupSql.Load();

            sql.Should().Contain("EXEC (N'CREATE SCHEMA [registration_pre_migration]')");
            sql.Should().Contain("N'DROP TABLE [registration_pre_migration].'", "an earlier backup is replaced");
            sql.Should().Contain("N'SELECT * INTO [registration_pre_migration].'");
            sql.Should().Contain("INTO [registration_pre_migration].[payments_unlinked]");
            sql.Should().Contain("WHERE p.[RegistrationSubmissionDataId] IS NOT NULL;");
            sql.Should().Contain("THROW 50000", "a count mismatch aborts before anything is cleared");
            sql.Should().NotContain("{", "ExecuteSqlRaw treats braces as format placeholders");
            sql.Should().NotContain("\nGO", "the script runs as one batch");
        }
    }
}
