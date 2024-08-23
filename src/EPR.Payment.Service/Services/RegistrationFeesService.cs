using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class RegistrationFeesService : IRegistrationFeesService
    {
        private readonly IRegistrationFeesRepository _registrationFeesRepository;

        public RegistrationFeesService(IRegistrationFeesRepository registrationFeesRepository)
        {
            _registrationFeesRepository = registrationFeesRepository ?? throw new ArgumentNullException(nameof(registrationFeesRepository));
        }
        public async Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(regulator))
            {
                throw new ArgumentException(FeesConstants.RegulatorCanNotBeNullOrEmpty, nameof(regulator));
            }

            return await _registrationFeesRepository.GetProducerResubmissionAmountByRegulatorAsync(regulator, cancellationToken);
        }
    }
}
