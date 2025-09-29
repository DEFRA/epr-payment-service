using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeSummaries
{
    public interface IFeeSummaryRepository
    {
        Task UpsertAsync(
            Guid externalId,
            string appRefNo,
            DateTimeOffset invoiceDate,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            int payerId,
            Guid fileId,
            IEnumerable<FeeSummary> items,
            CancellationToken cancellationToken);
    }
}
