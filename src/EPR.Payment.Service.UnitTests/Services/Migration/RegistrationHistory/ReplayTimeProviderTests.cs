using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class ReplayTimeProviderTests
    {
        [TestMethod]
        public void SetUtcNow_ReturnsThatTimeAsUtc()
        {
            var sut = new ReplayTimeProvider();
            var when = new DateTime(2025, 3, 5, 10, 2, 36, DateTimeKind.Unspecified);

            sut.SetUtcNow(when);

            sut.GetUtcNow().Should().Be(new DateTimeOffset(2025, 3, 5, 10, 2, 36, TimeSpan.Zero));
            sut.GetUtcNow().UtcDateTime.Kind.Should().Be(DateTimeKind.Utc);
        }

        [TestMethod]
        public void SetUtcNow_SameOrEarlierTime_StillAdvancesByOneTick()
        {
            var sut = new ReplayTimeProvider();
            var when = new DateTime(2025, 3, 5, 10, 0, 0, DateTimeKind.Utc);

            sut.SetUtcNow(when);
            var first = sut.GetUtcNow();
            sut.SetUtcNow(when);
            var second = sut.GetUtcNow();
            sut.SetUtcNow(when.AddDays(-1));
            var third = sut.GetUtcNow();

            second.Should().Be(first.AddTicks(1));
            third.Should().Be(second.AddTicks(1));
        }
    }
}
