using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeCalculatorService : IComplianceSchemeCalculatorService
    {
        private readonly ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal> _baseFeeCalculationStrategy;
        private readonly ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> _complianceSchemeOnlineMarketStrategy;
        private readonly ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal> _complianceSchemeLateFeeStrategy;
        private readonly ICSLateSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> _complianceSchemeLateSubsidiariesFeeStrategy;
        private readonly ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> _complianceSchemeMemberStrategy;
        private readonly IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;
        private readonly IPaymentsService _paymentsService;

        public ComplianceSchemeCalculatorService(
            ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal> baseFeeCalculationStrategy,
            ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> complianceSchemeOnlineMarketStrategy,
            ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal> complianceSchemeLateFeeStrategy,
            ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> complianceSchemeMemberStrategy,
            IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy,
            IPaymentsService paymentsService,
            ICSLateSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> complianceSchemeLateSubsidiariesFeeStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _complianceSchemeOnlineMarketStrategy = complianceSchemeOnlineMarketStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeOnlineMarketStrategy));
            _complianceSchemeLateFeeStrategy = complianceSchemeLateFeeStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeLateFeeStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _complianceSchemeMemberStrategy = complianceSchemeMemberStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeMemberStrategy));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _complianceSchemeLateSubsidiariesFeeStrategy = complianceSchemeLateSubsidiariesFeeStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeLateSubsidiariesFeeStrategy)); ;
        }

        public async Task<ComplianceSchemeFeesResponseDto> CalculateFeesAsync(ComplianceSchemeFeesRequestDto request, CancellationToken cancellationToken)
        {
            var regulatorType = RegulatorType.Create(request.Regulator);

            var response = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>()
            };

            decimal memberLateFee = await GetMemberLateFee(request, regulatorType, cancellationToken);

            foreach (var item in request.ComplianceSchemeMembers)
            {
                var complianceSchemeMemberWithRegulatorDto = new ComplianceSchemeMemberWithRegulatorDto
                {
                    Regulator = regulatorType,
                    MemberType = item.MemberType,
                    IsOnlineMarketplace = item.IsOnlineMarketplace,
                    IsLateFeeApplicable = item.IsLateFeeApplicable,
                    NumberOfSubsidiaries = item.NumberOfSubsidiaries,
                    NoOfSubsidiariesOnlineMarketplace = item.NoOfSubsidiariesOnlineMarketplace,
                    SubmissionDate = request.SubmissionDate,
                    NumberOfLateSubsidiaries = item.NumberOfLateSubsidiaries
                };

                var member = new ComplianceSchemeMembersWithFeesDto
                {
                    MemberId = item.MemberId,
                    MemberRegistrationFee = await _complianceSchemeMemberStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken),
                    MemberOnlineMarketPlaceFee = await _complianceSchemeOnlineMarketStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken),
                    SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken)
                };

                member.SubsidiariesFee = member.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees
                                         + member.SubsidiariesFeeBreakdown.FeeBreakdowns.Sum(i => i.TotalPrice);

                if (item.IsLateFeeApplicable)
                {
                    var subsidiariesLateFee = item.NumberOfSubsidiaries * memberLateFee;
                    member.MemberLateRegistrationFee = memberLateFee + subsidiariesLateFee;
                }

                member.SubsidiariesLateRegistrationFee = await _complianceSchemeLateSubsidiariesFeeStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken);

                member.TotalMemberFee = member.MemberRegistrationFee
                                        + member.MemberOnlineMarketPlaceFee
                                        + member.SubsidiariesFee
                                        + member.MemberLateRegistrationFee
                                        + member.SubsidiariesLateRegistrationFee;

                // Add to response collection
                response.ComplianceSchemeMembersWithFees.Add(member);
            }

            response.TotalFee = response.ComplianceSchemeRegistrationFee
                                + response.ComplianceSchemeMembersWithFees.Sum(m => m.TotalMemberFee);
            response.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);
            response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

            return response;
        }

        private async Task<decimal> GetMemberLateFee(ComplianceSchemeFeesRequestDto request, RegulatorType regulatorType, CancellationToken cancellationToken)
        {
            if (request.ComplianceSchemeMembers.Exists(m => m.IsLateFeeApplicable))
            {
                return await _complianceSchemeLateFeeStrategy.CalculateFeeAsync(
                    new ComplianceSchemeLateFeeRequestDto
                    {
                        Regulator = regulatorType,
                        SubmissionDate = request.SubmissionDate,
                        IsLateFeeApplicable = true
                    },
                    cancellationToken);
            }
            return 0;
        }

    }
}