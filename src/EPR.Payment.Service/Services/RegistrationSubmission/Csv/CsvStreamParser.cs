using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;
using CsvHelper.Configuration;

namespace EPR.Payment.Service.Services.RegistrationSubmission.Csv
{
    public class CsvStreamParser : ICsvStreamParser
    {
        private static readonly CsvConfiguration Configuration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim,
        };

        public async IAsyncEnumerable<T> ParseAsync<T>(
            Stream stream,
            ClassMap<T>? classMap,
            [EnumeratorCancellation] CancellationToken cancellationToken)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(stream);

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, Configuration);

            if (classMap is not null)
            {
                csv.Context.RegisterClassMap(classMap);
            }

            await foreach (var record in csv.GetRecordsAsync<T>(cancellationToken))
            {
                yield return record;
            }
        }
    }
}
