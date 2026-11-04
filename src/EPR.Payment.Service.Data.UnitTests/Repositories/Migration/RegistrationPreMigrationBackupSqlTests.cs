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

            sql.Should().Contain("N'DELETE FROM [registration_pre_migration].'", "an earlier backup is replaced");
            sql.Should().Contain("N'INSERT INTO [registration_pre_migration].'");
            sql.Should().Contain("INSERT INTO [registration_pre_migration].[payments_unlinked]");
            sql.Should().Contain("WHERE p.[RegistrationSubmissionDataId] IS NOT NULL;");
            sql.Should().Contain("apply the EF Core migrations first", "missing backup tables abort before anything is cleared");
            sql.Should().Contain("THROW 50000", "a count mismatch aborts before anything is cleared");

            // The service's login cannot create or drop objects; the EF Core migration creates them.
            var statements = string.Join('\n', sql.Split('\n').Where(l => !l.TrimStart().StartsWith("--", StringComparison.Ordinal)));
            var body = statements[(statements.IndexOf("*/", StringComparison.Ordinal) + 2)..];
            body.Should().NotContainAny("CREATE ", "DROP ", "ALTER ", "TRUNCATE ", "SELECT * INTO");
            sql.Should().NotContain("{", "ExecuteSqlRaw treats braces as format placeholders");
            sql.Should().NotContain("\nGO", "the script runs as one batch");
        }
    }
}
