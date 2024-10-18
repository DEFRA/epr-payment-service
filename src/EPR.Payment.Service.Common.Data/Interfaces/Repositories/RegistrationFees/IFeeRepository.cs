using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IFeeRepository
    {
        Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetLateFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}
