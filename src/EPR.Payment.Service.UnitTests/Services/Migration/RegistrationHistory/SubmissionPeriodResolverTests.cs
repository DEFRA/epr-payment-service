using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class SubmissionPeriodResolverTests
    {
        private static readonly IReadOnlyList<SubmissionPeriod> Periods = new List<SubmissionPeriod>
        {
            new() { Id = 1, WindowType = "Cso", RegistrationYear = 2025 },
            new() { Id = 2, WindowType = "Direct", RegistrationYear = 2025 },
            new() { Id = 3, WindowType = "CsoLargeProducer", RegistrationYear = 2026 },
            new() { Id = 4, WindowType = "CsoSmallProducer", RegistrationYear = 2026 },
            new() { Id = 5, WindowType = "DirectLargeProducer", RegistrationYear = 2026 },
            new() { Id = 6, WindowType = "DirectSmallProducer", RegistrationYear = 2026 },
        };

        [DataTestMethod]
        [DataRow("January to December 2025", null, true, 1)]
        [DataRow("January to December 2025", "CsoSmallProducer", false, 2)]
        [DataRow("January to December 2026", "CsoLargeProducer", true, 3)]
        [DataRow("January to December 2026", "CsoSmallProducer", true, 4)]
        [DataRow("January to December 2026", "DirectLargeProducer", false, 5)]
        [DataRow("January to December 2026", "DirectSmallProducer", false, 6)]
        [DataRow("January to December 2026", null, false, 5)]
        [DataRow(" January to December 2026 ", "directsmallproducer", false, 6)]
        public void Resolve_MapsPeriodJourneyAndSchemeToLookupId(string period, string? journey, bool isComplianceScheme, int expectedId)
        {
            SubmissionPeriodResolver.Resolve(period, journey, isComplianceScheme, Periods).Should().Be(expectedId);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("Jan")]
        [DataRow("January to December")]
        [DataRow("January to December 2024")]
        [DataRow("January to December 2030")]
        public void Resolve_WhenPeriodUnparseableOrUnseeded_ReturnsNull(string? period)
        {
            SubmissionPeriodResolver.Resolve(period, "DirectLargeProducer", false, Periods).Should().BeNull();
        }

        [TestMethod]
        public void Resolve_NullPeriods_Throws()
        {
            ((Action)(() => SubmissionPeriodResolver.Resolve("January to December 2026", null, false, null!)))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
