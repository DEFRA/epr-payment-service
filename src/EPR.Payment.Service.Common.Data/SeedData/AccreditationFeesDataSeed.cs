using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class AccreditationFeesDataSeed
    {        
        private static readonly DateTime effectiveFromDateForYear2024 = DateTime.ParseExact("01/09/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDateForYear9999 = DateTime.ParseExact("31/08/9999 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        public static void SeedAccreditationFees(EntityTypeBuilder<AccreditationFee> builder)
        {
            SeedExporterAccreditationFeesData(builder);
            SeedReprocessorAccreditationFeesData(builder);
        }

        private static void SeedExporterAccreditationFeesData(EntityTypeBuilder<AccreditationFee> builder)
        {
            
        }

        private static void SeedReprocessorAccreditationFeesData(EntityTypeBuilder<AccreditationFee> builder)
        {
            
        }
    }
}
