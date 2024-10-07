﻿using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class SubsidiariesFeeCalculationStrategy : BaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public SubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        protected override async Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetFirstBandFeeAsync(regulator, cancellationToken);
        }
        protected override async Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetSecondBandFeeAsync(regulator, cancellationToken);
        }

        protected override async Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetThirdBandFeeAsync(regulator, cancellationToken);
        }

        protected override async Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetOnlineMarketFeeAsync(regulator, cancellationToken);
        }

        protected override int GetNoOfOMPSubsidiaries(ProducerRegistrationFeesRequestDto request)
        {
            return request.NoOfSubsidiariesOnlineMarketplace;
        }

        protected override int GetNoOfSubsidiaries(ProducerRegistrationFeesRequestDto request)
        {
            return request.NumberOfSubsidiaries;
        }

        protected override RegulatorType GetRegulator(ProducerRegistrationFeesRequestDto request)
        {
            return RegulatorType.Create(request.Regulator);
        }
    }
}
