using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public static class RegistrationFeeSnapshotProjector
    {
        public static RegistrationFeesResponseDto ToProducerResponse(RegistrationFeeSnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            var breakdown = new SubsidiariesFeeBreakdown();
            var response = new RegistrationFeesResponseDto
            {
                SubsidiariesFeeBreakdown = breakdown,
                TotalFee = snapshot.TotalFee,
                MemberId = string.Empty,
            };

            foreach (var line in snapshot.LineItems)
            {
                switch (line.FeeTypeId)
                {
                    case FeeTypeIds.ProducerRegistrationFee:
                        response.ProducerRegistrationFee = line.Amount;
                        break;
                    case FeeTypeIds.ProducerOnlineMarketplaceFee:
                        response.ProducerOnlineMarketPlaceFee = line.Amount;
                        break;
                    case FeeTypeIds.ProducerClosedLoopRecyclingFee:
                        response.ProducerClosedLoopRecyclingFee = line.Amount;
                        break;
                    case FeeTypeIds.ProducerLateRegistrationFee:
                        response.ProducerLateRegistrationFee = line.Amount;
                        break;
                    case FeeTypeIds.SubsidiaryFee:
                        breakdown.FeeBreakdowns.Add(new FeeBreakdown
                        {
                            BandNumber = line.BandNumber ?? 0,
                            UnitCount = line.Quantity ?? 0,
                            UnitPrice = line.UnitPrice ?? 0m,
                            TotalPrice = line.Amount,
                        });
                        break;
                    case FeeTypeIds.SubsidiaryOnlineMarketplaceFee:
                        breakdown.TotalSubsidiariesOMPFees = line.Amount;
                        breakdown.UnitOMPFees = line.UnitPrice ?? 0m;
                        breakdown.CountOfOMPSubsidiaries = line.Quantity ?? 0;
                        break;
                    case FeeTypeIds.SubsidiaryClosedLoopRecyclingFee:
                        breakdown.TotalSubsidiariesClosedLoopRecyclingFees = line.Amount;
                        breakdown.UnitClosedLoopRecyclingFees = line.UnitPrice ?? 0m;
                        breakdown.CountOfClosedLoopRecyclingSubsidiaries = line.Quantity ?? 0;
                        break;
                }
            }

            response.SubsidiariesFee = breakdown.FeeBreakdowns.Sum(f => f.TotalPrice);

            return response;
        }

        public static ComplianceSchemeFeesResponseDto ToComplianceSchemeResponse(RegistrationFeeSnapshot snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            var response = new ComplianceSchemeFeesResponseDto
            {
                TotalFee = snapshot.TotalFee,
            };

            var memberGroups = snapshot.LineItems
                .Where(l => l.MemberId is not null)
                .GroupBy(l => l.MemberId!, StringComparer.Ordinal);

            foreach (var line in snapshot.LineItems.Where(l => l.MemberId is null))
            {
                if (line.FeeTypeId == FeeTypeIds.ComplianceSchemeRegistrationFee)
                {
                    response.ComplianceSchemeRegistrationFee = line.Amount;
                }
            }

            foreach (var group in memberGroups)
            {
                var breakdown = new SubsidiariesFeeBreakdown();
                var member = new ComplianceSchemeMembersWithFeesDto
                {
                    MemberId = group.Key,
                    SubsidiariesFeeBreakdown = breakdown,
                };

                foreach (var line in group)
                {
                    switch (line.FeeTypeId)
                    {
                        case FeeTypeIds.MemberRegistrationFee:
                            member.MemberRegistrationFee = line.Amount;
                            break;
                        case FeeTypeIds.MemberOnlineMarketplaceFee:
                            member.MemberOnlineMarketPlaceFee = line.Amount;
                            break;
                        case FeeTypeIds.MemberClosedLoopRecyclingFee:
                            member.MemberClosedLoopRecyclingFee = line.Amount;
                            break;
                        case FeeTypeIds.MemberLateRegistrationFee:
                            member.MemberLateRegistrationFee = line.Amount;
                            break;
                        case FeeTypeIds.SubsidiaryFee:
                            breakdown.FeeBreakdowns.Add(new FeeBreakdown
                            {
                                BandNumber = line.BandNumber ?? 0,
                                UnitCount = line.Quantity ?? 0,
                                UnitPrice = line.UnitPrice ?? 0m,
                                TotalPrice = line.Amount,
                            });
                            break;
                        case FeeTypeIds.SubsidiaryOnlineMarketplaceFee:
                            breakdown.TotalSubsidiariesOMPFees = line.Amount;
                            breakdown.UnitOMPFees = line.UnitPrice ?? 0m;
                            breakdown.CountOfOMPSubsidiaries = line.Quantity ?? 0;
                            break;
                        case FeeTypeIds.SubsidiaryClosedLoopRecyclingFee:
                            breakdown.TotalSubsidiariesClosedLoopRecyclingFees = line.Amount;
                            breakdown.UnitClosedLoopRecyclingFees = line.UnitPrice ?? 0m;
                            breakdown.CountOfClosedLoopRecyclingSubsidiaries = line.Quantity ?? 0;
                            break;
                    }
                }

                member.SubsidiariesFee = breakdown.FeeBreakdowns.Sum(f => f.TotalPrice);
                member.TotalMemberFee = member.MemberRegistrationFee
                                        + member.MemberOnlineMarketPlaceFee
                                        + member.MemberClosedLoopRecyclingFee
                                        + member.MemberLateRegistrationFee
                                        + member.SubsidiariesFee
                                        + breakdown.TotalSubsidiariesOMPFees
                                        + breakdown.TotalSubsidiariesClosedLoopRecyclingFees;

                response.ComplianceSchemeMembersWithFees.Add(member);
            }

            return response;
        }
    }
}
