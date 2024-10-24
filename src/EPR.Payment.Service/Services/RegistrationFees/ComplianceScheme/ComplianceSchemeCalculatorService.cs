using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeCalculatorService : IComplianceSchemeCalculatorService
    {
        private readonly ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal> _baseFeeCalculationStrategy;
        private readonly ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> _complianceSchemeOnlineMarketStrategy;
        private readonly ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> _complianceSchemeMemberStrategy;
        private readonly IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;

        public ComplianceSchemeCalculatorService(
            ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal> baseFeeCalculationStrategy,
            ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> complianceSchemeOnlineMarketStrategy,
            ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal> complianceSchemeMemberStrategy,
            IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _complianceSchemeOnlineMarketStrategy = complianceSchemeOnlineMarketStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeOnlineMarketStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _complianceSchemeMemberStrategy = complianceSchemeMemberStrategy ?? throw new ArgumentNullException(nameof(complianceSchemeMemberStrategy));
        }

        public async Task<ComplianceSchemeFeesResponseDto> CalculateFeesAsync(ComplianceSchemeFeesRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var regulatorType = RegulatorType.Create(request.Regulator);

                var response = new ComplianceSchemeFeesResponseDto
                {
                    ComplianceSchemeRegistrationFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                    ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>()
                };

                foreach (var item in request.ComplianceSchemeMembers)
                {
                    var complianceSchemeMemberWithRegulatorDto = new ComplianceSchemeMemberWithRegulatorDto
                    {
                        Regulator = regulatorType,
                        MemberType = item.MemberType,
                        IsOnlineMarketplace = item.IsOnlineMarketplace,
                        NumberOfSubsidiaries = item.NumberOfSubsidiaries,
                        NoOfSubsidiariesOnlineMarketplace = item.NoOfSubsidiariesOnlineMarketplace,
                        SubmissionDate = request.SubmissionDate
                    };

                    var member = new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = item.MemberId,
                        MemberRegistrationFee = await _complianceSchemeMemberStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken),
                        MemberOnlineMarketPlaceFee = await _complianceSchemeOnlineMarketStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken),
                        SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(complianceSchemeMemberWithRegulatorDto, cancellationToken)
                    };

                    member.SubsidiariesFee = member.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees + member.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum();
                    member.TotalMemberFee = member.MemberRegistrationFee + member.MemberOnlineMarketPlaceFee + member.SubsidiariesFee;

                    response.ComplianceSchemeMembersWithFees.Add(member);
                }
                response.TotalFee = response.ComplianceSchemeRegistrationFee + response.ComplianceSchemeMembersWithFees.Select(i => i.TotalMemberFee).Sum();
                response.PreviousPayment = 0;// TODO: This will not be 0 but calculated once the database schema is changes are ready
                response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

                return response;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(ComplianceSchemeFeeCalculationExceptions.CalculationError, ex);
            }
        }
    }
}