using System.Net;
using System.Net.Http.Json;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace EPR.Payment.Service.IntegrationTests.Controllers;

public class ProducerFeesControllerTests : IntegrationTestBase
{
    private static readonly DateTime ValidSubmissionDate = new(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    [Test]
    public async Task CalculateFees_WithFileId_ReturnsPreviousPaymentByFileId()
    {
        // Arrange - seed a successful payment linked to a specific FileId
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 500m, reference: "REF-PROD-001");

        var request = BuildRequest("REF-PROD-001", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(500m);
        result.OutstandingPayment.Should().Be(result.TotalFee - 500m);
        result.TotalFee.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CalculateFees_WithFileId_WhenNoMatchingPayment_FallsBackToReferencePayment()
    {
        // Arrange - payment exists on the reference but with a different FileId
        await SeedPaymentAsync(fileId: Guid.NewGuid(), amount: 300m, reference: "REF-PROD-002");

        var unmatchedFileId = Guid.NewGuid();
        var request = BuildRequest("REF-PROD-002", fileId: unmatchedFileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // FileId lookup returns 0 → falls back to reference → finds 300
        result!.PreviousPayment.Should().Be(300m);
    }

    [Test]
    public async Task CalculateFees_WithoutFileId_UsesPreviousPaymentByReference()
    {
        // Arrange - payment only linked via reference, no FileId
        await SeedPaymentAsync(fileId: null, amount: 200m, reference: "REF-PROD-003");

        var request = BuildRequest("REF-PROD-003", fileId: null);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(200m);
    }

    [Test]
    public async Task CalculateFees_WithFileId_DoesNotDoubleCountWhenReferenceAlsoHasPayments()
    {
        // Arrange - one payment matches FileId; another matches only the reference
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 400m, reference: "REF-PROD-004");
        await SeedPaymentAsync(fileId: null, amount: 600m, reference: "REF-PROD-004");

        var request = BuildRequest("REF-PROD-004", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // FileId lookup returns 400 (non-zero), so reference fallback is skipped
        result!.PreviousPayment.Should().Be(400m);
    }

    [Test]
    public async Task CalculateFees_WithFileId_OnlyCountsSuccessfulPayments()
    {
        // Arrange - payment exists for FileId but with Failed status
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 400m, reference: "REF-PROD-005", status: Status.Failed);

        var request = BuildRequest("REF-PROD-005", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        // Failed payment not counted, FileId returns 0, reference also returns 0 → PreviousPayment=0
        result!.PreviousPayment.Should().Be(0m);
    }

    [Test]
    public async Task CalculateFees_WithFileId_SumsMultipleSuccessfulPayments()
    {
        // Arrange - two successful payments share the same FileId
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 200m, reference: "REF-PROD-006");
        await SeedPaymentAsync(fileId, amount: 150m, reference: "REF-PROD-006");

        var request = BuildRequest("REF-PROD-006", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/producer/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RegistrationFeesResponseDto>();
        result!.PreviousPayment.Should().Be(350m);
    }

    [Test]
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

    private static ProducerRegistrationFeesRequestDto BuildRequest(string reference, Guid? fileId = null) =>
        new()
        {
            ProducerType = "Large",
            Regulator = "GB-ENG",
            ApplicationReferenceNumber = reference,
            SubmissionDate = ValidSubmissionDate,
            NumberOfSubsidiaries = 0,
            IsProducerOnlineMarketplace = false,
            IsLateFeeApplicable = false,
            FileId = fileId
        };
}
