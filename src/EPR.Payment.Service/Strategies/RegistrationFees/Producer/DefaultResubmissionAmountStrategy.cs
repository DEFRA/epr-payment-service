﻿using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class DefaultResubmissionAmountStrategy : IResubmissionAmountStrategy
    {
        private readonly IProducerFeesRepository _feesRepository;

        public DefaultResubmissionAmountStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal?> GetResubmissionAsync(RegulatorDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty");
            }

            var regulatorType = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetResubmissionAsync(regulatorType, cancellationToken);
        }
    }
}
