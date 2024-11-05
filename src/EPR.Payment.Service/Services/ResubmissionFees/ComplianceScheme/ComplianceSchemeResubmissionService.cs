using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionService : IComplianceSchemeResubmissionService
    {
        private readonly IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal> _resubmissionFeeStrategy;

        public ComplianceSchemeResubmissionService(
            IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal> resubmissionFeeStrategy)
        {
            _resubmissionFeeStrategy = resubmissionFeeStrategy ?? throw new ArgumentNullException(nameof(resubmissionFeeStrategy));
        }

        public async Task<ComplianceSchemeResubmissionFeeResult> CalculateResubmissionFeeAsync(ComplianceSchemeResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
            if (request.MemberCount < 1)
            {
                throw new ArgumentException(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidMemberCountError, request.MemberCount));
            }

            var baseFee = await _resubmissionFeeStrategy.CalculateFeeAsync(request, cancellationToken);

            // Hard-coding previous payments for now, can be updated later.
            var previousPayments = 0m;

            var totalFee = baseFee * request.MemberCount;
            var outstandingPayment = totalFee - previousPayments;

            return new ComplianceSchemeResubmissionFeeResult
            {
                TotalResubmissionFee = totalFee,
                PreviousPayments = previousPayments,
                OutstandingPayment = outstandingPayment > 0 ? outstandingPayment : 0,
                MemberCount = request.MemberCount
            };
        }
    }
}