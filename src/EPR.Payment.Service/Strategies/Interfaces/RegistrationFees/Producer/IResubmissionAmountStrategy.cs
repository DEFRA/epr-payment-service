﻿using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer
{
    public interface IResubmissionAmountStrategy : IFeeCalculationStrategy<RegulatorDto>
    {
    }
}