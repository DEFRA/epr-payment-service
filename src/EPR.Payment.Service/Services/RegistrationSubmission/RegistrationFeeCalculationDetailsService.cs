using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegistrationFeeCalculationDetailsService : IRegistrationFeeCalculationDetailsService
    {
        private readonly IRegistrationSubmissionDataRepository _repository;

        public RegistrationFeeCalculationDetailsService(IRegistrationSubmissionDataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<RegistrationFeeCalculationDetailsDto>> GetAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            var snapshot = await _repository.GetLatestWithProducersAndSubsidiariesAsync(submissionId, cancellationToken);
            if (snapshot is null || snapshot.Producers.Count == 0)
            {
                return Array.Empty<RegistrationFeeCalculationDetailsDto>();
            }

            return snapshot.Producers.Select(MapProducer).ToList();
        }

        private static RegistrationFeeCalculationDetailsDto MapProducer(RegistrationSubmissionProducer producer) => new()
        {
            OrganisationId = producer.OrganisationId,
            OrganisationSize = producer.OrganisationSize,
            IsOnlineMarketplace = producer.IsOnlineMarketplace,
            IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
            IsNewJoiner = producer.IsNewJoiner,
            NationId = producer.NationId,
            NumberOfSubsidiaries = producer.Subsidiaries.Count,
            NumberOfSubsidiariesBeingOnlineMarketPlace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
            NumberOfSubsidiariesBeingClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
        };
    }
}
