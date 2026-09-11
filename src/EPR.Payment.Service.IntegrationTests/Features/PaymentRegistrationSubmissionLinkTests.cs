using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Features;

/// <summary>
/// <c>Payment.RegistrationSubmissionDataId</c> is resolved from the real
/// InsertOnlinePayment production code path (<c>OnlinePaymentsService</c>), matched by exact
/// application-reference-number string - everything downstream (previous-payment lookups, the
/// snapshot's own outstanding-payment calculation) depends on this link being correct.
/// </summary>
public class PaymentRegistrationSubmissionLinkTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GIVEN_matching_registration_submission_WHEN_payment_inserted_THEN_link_is_resolved()
    {
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var built = await Builder.RegistrationSubmissionData()
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Build();

        var externalPaymentId = await InsertOnlinePayment(applicationReferenceNumber);

        var linkedId = await GetRegistrationSubmissionDataIdForPayment(externalPaymentId);

        linkedId.Should().Be(built.Id);
    }

    [Fact]
    public async Task GIVEN_no_matching_registration_submission_WHEN_payment_inserted_THEN_link_is_null()
    {
        var applicationReferenceNumber = NewApplicationReferenceNumber(); // nothing seeded for this reference

        var externalPaymentId = await InsertOnlinePayment(applicationReferenceNumber);

        var linkedId = await GetRegistrationSubmissionDataIdForPayment(externalPaymentId);

        linkedId.Should().BeNull();
    }

    [Fact]
    public async Task GIVEN_multiple_submission_rows_sharing_a_reference_WHEN_payment_inserted_THEN_link_resolves_to_the_latest()
    {
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var earlier = await Builder.RegistrationSubmissionData()
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Build();

        // A resubmission cycle for the same application reference, created strictly after the
        // first - CreatedDate is set by the database default, so a short delay keeps the two
        // rows unambiguously ordered without relying on clock resolution.
        await Task.Delay(TimeSpan.FromMilliseconds(50));

        var later = await Builder.RegistrationSubmissionData()
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Build();

        var externalPaymentId = await InsertOnlinePayment(applicationReferenceNumber);

        var linkedId = await GetRegistrationSubmissionDataIdForPayment(externalPaymentId);

        linkedId.Should().Be(later.Id).And.NotBe(earlier.Id);
    }

    private async Task<Guid> InsertOnlinePayment(string reference)
    {
        var request = new OnlinePaymentInsertRequestDto
        {
            UserId = Guid.NewGuid(),
            OrganisationId = Guid.NewGuid(),
            Reference = reference,
            Regulator = RegulatorConstants.GBENG,
            Amount = 100,
            ReasonForPayment = ReasonForPaymentConstants.RegistrationFee,
            Status = Status.Success,
        };

        var response = await Client.PostAsJsonAsync("/api/v1/online-payments", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());

        return await response.ReadJson<Guid>();
    }

    private async Task<Guid?> GetRegistrationSubmissionDataIdForPayment(Guid externalPaymentId)
    {
        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Payment
            .Where(p => p.ExternalPaymentId == externalPaymentId)
            .Select(p => p.RegistrationSubmissionDataId)
            .SingleAsync();
    }

    private static string NewApplicationReferenceNumber() => $"PEPR{Guid.NewGuid():N}"[..15].ToUpperInvariant();
}
