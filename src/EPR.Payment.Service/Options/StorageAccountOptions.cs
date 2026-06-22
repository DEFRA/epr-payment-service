using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Options
{
    [ExcludeFromCodeCoverage]
    public class StorageAccountOptions
    {
        public const string SectionName = "StorageAccount";

        public string ConnectionString { get; set; } = string.Empty;

        public string RegistrationContainer { get; set; } = string.Empty;
    }
}
