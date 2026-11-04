namespace EPR.Payment.Service.Common.Data.Repositories.Migration
{
    /// <summary>
    /// Scripts/Migration/backup-registration-pre-migration.sql, embedded so the migration runs the same script a DBA can run by hand.
    /// </summary>
    public static class RegistrationPreMigrationBackupSql
    {
        private const string ResourceName = "RegistrationPreMigrationBackup.sql";

        private static readonly Lazy<string> Sql = new(() =>
        {
            using var stream = typeof(RegistrationPreMigrationBackupSql).Assembly.GetManifestResourceStream(ResourceName)
                ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' not found.");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        });

        public static string Load() => Sql.Value;
    }
}
