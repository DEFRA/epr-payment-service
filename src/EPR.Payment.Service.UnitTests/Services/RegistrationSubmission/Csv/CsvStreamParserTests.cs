using EPR.Payment.Service.Services.RegistrationSubmission.Csv;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Maps;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models;
using EPR.Payment.Service.UnitTests.Services.RegistrationSubmission.TestHelpers;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission.Csv
{
    [TestClass]
    public class CsvStreamParserTests
    {
        private readonly CsvStreamParser _sut = new();

        [TestMethod]
        public async Task ParseAsync_HeaderAndOneRow_ReturnsMappedRow()
        {
            using var stream = RegistrationCsvFixtureFactory.BuildCsvStream(
                RegistrationCsvFixtureFactory.Producer("ORG-1", "EN", "Large", "Primary", "2026-04-01", "Yes"));

            var rows = new List<RegistrationCsvRow>();
            await foreach (var row in _sut.ParseAsync(stream, new RegistrationCsvRowMap(), CancellationToken.None))
            {
                rows.Add(row);
            }

            rows.Should().HaveCount(1);
            rows[0].OrganisationId.Should().Be("ORG-1");
            rows[0].HomeNationCode.Should().Be("EN");
            rows[0].OrganisationSize.Should().Be("Large");
            rows[0].PackagingActivityOm.Should().Be("Primary");
            rows[0].JoinerDate.Should().Be("2026-04-01");
            rows[0].ClosedLoopRegistration.Should().Be("Yes");
        }

        [TestMethod]
        public async Task ParseAsync_MultipleRows_ReturnsAll()
        {
            using var stream = RegistrationCsvFixtureFactory.BuildCsvStream(
                RegistrationCsvFixtureFactory.Producer("ORG-A"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-A", "SUB-1"),
                RegistrationCsvFixtureFactory.Producer("ORG-B"));

            var rows = new List<RegistrationCsvRow>();
            await foreach (var row in _sut.ParseAsync(stream, new RegistrationCsvRowMap(), CancellationToken.None))
            {
                rows.Add(row);
            }

            rows.Select(r => r.OrganisationId).Should().Equal("ORG-A", "ORG-A", "ORG-B");
            rows.Select(r => r.SubsidiaryId).Should().Equal("", "SUB-1", "");
        }

        [TestMethod]
        public async Task ParseAsync_NullStream_Throws()
        {
            Func<Task> act = async () =>
            {
                await foreach (var _ in _sut.ParseAsync<RegistrationCsvRow>(null!, new RegistrationCsvRowMap(), CancellationToken.None))
                {
                }
            };

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
