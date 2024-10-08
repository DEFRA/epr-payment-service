﻿using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer
{
    public interface IProducerResubmissionService
    {
        Task<decimal?> GetResubmissionAsync(RegulatorDto request, CancellationToken cancellationToken);
    }
}
