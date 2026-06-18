using CsvHelper.Configuration;

namespace EPR.Payment.Service.Services.RegistrationSubmission.Csv
{
    public interface ICsvStreamParser
    {
        IAsyncEnumerable<T> ParseAsync<T>(Stream stream, ClassMap<T>? classMap, CancellationToken cancellationToken)
            where T : class;
    }
}
