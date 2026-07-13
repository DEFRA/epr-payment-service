using System.Net;
using System.Net.Http.Json;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using AwesomeAssertions;

namespace EPR.Payment.Service.IntegrationTests.Controllers;

public class ProducerFeesControllerTests(ServiceFixture fixture)  : IntegrationTestBase(fixture)
{
    private static readonly DateTime ValidSubmissionDate = new(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_ReturnsPreviousPaymentByBlobName()
    {
        // Arrange - seed a successful payment whose reference equals the blob name (join key)
        var blobName = $"blob-prod-001-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 500m, reference: blobName);

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(500m);
        result.OutstandingPayment.Should().Be(result.TotalFee - 500m);
        result.TotalFee.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_WhenNoMatchingPayment_FallsBackToReferencePayment()
    {
        // Arrange - payment exists on the reference but no RegistrationSubmissionData for the blob name
        var reference = $"REF-PROD-002-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: null, amount: 300m, reference: reference);

        var unknownBlobName = $"unknown-blob-{Guid.NewGuid():N}";
        var request = BuildRequest(applicationReferenceNumber: reference, registrationBlobName: unknownBlobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // Blob name lookup returns 0 → falls back to reference → finds 300
        result!.PreviousPayment.Should().Be(300m);
    }

    [Fact]
    public async Task CalculateFees_WithoutRegistrationBlobName_UsesPreviousPaymentByReference()
    {
        // Arrange - payment only linked via reference, no blob name
        var reference = $"REF-PROD-003-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: null, amount: 200m, reference: reference);

        var request = BuildRequest(applicationReferenceNumber: reference, registrationBlobName: null);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(200m);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_DoesNotDoubleCountWhenOtherPaymentsExist()
    {
        // Arrange - payment1 matches blob name; payment2 has a different reference
        var blobName = $"blob-prod-004-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 400m, reference: blobName);
        await SeedPaymentAsync(registrationBlobName: null, amount: 600m, reference: $"other-ref-{Guid.NewGuid():N}");

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // Blob name lookup returns 400 (non-zero), so reference fallback is skipped
        result!.PreviousPayment.Should().Be(400m);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_OnlyCountsSuccessfulPayments()
    {
        // Arrange - payment exists for blob name but with Failed status
        var blobName = $"blob-prod-005-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 400m, reference: blobName, status: Status.Failed);

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // Failed payment not counted → PreviousPayment = 0
        result!.PreviousPayment.Should().Be(0m);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_SumsMultipleSuccessfulPayments()
    {
        // Arrange - two successful payments share the same blob name / reference
        var blobName = $"blob-prod-006-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 200m, reference: blobName);
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 150m, reference: blobName);

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(350m);
    }

    [Fact]
    public async Task CalculateFees_WithInvalidRegulator_Returns400()
    {
        // Arrange
        var request = new ProducerRegistrationFeesRequestDto
        {
            ProducerType = "Large",
            Regulator = "INVALID",
            ApplicationReferenceNumber = "REF-PROD-007",
            SubmissionDate = ValidSubmissionDate
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private static ProducerRegistrationFeesRequestDto BuildRequest(string applicationReferenceNumber, string? registrationBlobName = null) =>
        new()
        {
            ProducerType = "Large",
            Regulator = "GB-ENG",
            ApplicationReferenceNumber = applicationReferenceNumber,
            SubmissionDate = ValidSubmissionDate,
            NumberOfSubsidiaries = 0,
            IsProducerOnlineMarketplace = false,
            IsLateFeeApplicable = false,
            RegistrationBlobName = registrationBlobName
        };
}
