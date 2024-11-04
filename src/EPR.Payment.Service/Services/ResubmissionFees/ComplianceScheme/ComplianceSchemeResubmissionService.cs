using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionService : IComplianceSchemeResubmissionService
    {
        private readonly IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal> _resubmissionFeeStrategy;
        private readonly IPaymentsService  _paymentsService;

        public ComplianceSchemeResubmissionService(
            IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal> resubmissionFeeStrategy,
            IPaymentsService paymentsService)
        {
            _resubmissionFeeStrategy = resubmissionFeeStrategy ?? throw new ArgumentNullException(nameof(resubmissionFeeStrategy));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        public async Task<ComplianceSchemeResubmissionFeeResult> CalculateResubmissionFeeAsync(ComplianceSchemeResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
            if (request.MemberCount < 1)
            {
                throw new ArgumentException(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidMemberCountError, request.MemberCount));
            }

            var baseFee = await _resubmissionFeeStrategy.CalculateFeeAsync(request, cancellationToken);

            decimal previousPayments = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, cancellationToken);

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