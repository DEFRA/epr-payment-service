using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class RegulatorDecisionMapperTests
    {
        [DataTestMethod]
        [DataRow("Accepted", RegistrationEventNames.AcceptedByRegulator)]
        [DataRow("Rejected", RegistrationEventNames.RejectedByRegulator)]
        [DataRow("Queried", RegistrationEventNames.QueriedByRegulator)]
        [DataRow("Cancelled", RegistrationEventNames.CancelledByRegulator)]
        [DataRow(" accepted ", RegistrationEventNames.AcceptedByRegulator)]
        public void TryMap_KnownDecision_ReturnsEventName(string decision, string expected)
        {
            RegulatorDecisionMapper.TryMap(decision, out var eventName).Should().BeTrue();
            eventName.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("Granted")]
        [DataRow("Refused")]
        public void TryMap_UnknownDecision_ReturnsFalse(string? decision)
        {
            RegulatorDecisionMapper.TryMap(decision, out var eventName).Should().BeFalse();
            eventName.Should().BeEmpty();
        }
    }
}
