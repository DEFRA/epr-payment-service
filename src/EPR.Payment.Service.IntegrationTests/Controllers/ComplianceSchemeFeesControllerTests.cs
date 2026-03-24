using System.Net;
using System.Net.Http.Json;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace EPR.Payment.Service.IntegrationTests.Controllers;

public class ComplianceSchemeFeesControllerTests : IntegrationTestBase
{
    private static readonly DateTime ValidSubmissionDate = new(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    [Test]
    public async Task CalculateFees_WithFileId_ReturnsPreviousPaymentByFileId()
    {
        // Arrange - seed a successful payment linked to a specific FileId
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 750m, reference: "REF-CS-001");

        var request = BuildRequest("REF-CS-001", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        result!.PreviousPayment.Should().Be(750m);
        result.OutstandingPayment.Should().Be(result.TotalFee - 750m);
        result.TotalFee.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CalculateFees_WithFileId_WhenNoMatchingPayment_FallsBackToReferencePayment()
    {
        // Arrange - payment exists on the reference but with a different FileId
        await SeedPaymentAsync(fileId: Guid.NewGuid(), amount: 400m, reference: "REF-CS-002");

        var unmatchedFileId = Guid.NewGuid();
        var request = BuildRequest("REF-CS-002", fileId: unmatchedFileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        // FileId lookup returns 0 → falls back to reference → finds 400
        result!.PreviousPayment.Should().Be(400m);
    }

    [Test]
    public async Task CalculateFees_WithoutFileId_UsesPreviousPaymentByReference()
    {
        // Arrange - payment only linked via reference, no FileId
        await SeedPaymentAsync(fileId: null, amount: 250m, reference: "REF-CS-003");

        var request = BuildRequest("REF-CS-003", fileId: null);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        result!.PreviousPayment.Should().Be(250m);
    }

    [Test]
    public async Task CalculateFees_WithFileId_DoesNotDoubleCountWhenReferenceAlsoHasPayments()
    {
        // Arrange - one payment matches FileId; another matches only the reference
        var fileId = Guid.NewGuid();
        await SeedPaymentAsync(fileId, amount: 600m, reference: "REF-CS-004");
        await SeedPaymentAsync(fileId: null, amount: 800m, reference: "REF-CS-004");

        var request = BuildRequest("REF-CS-004", fileId: fileId);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        // FileId lookup returns 600 (non-zero), so reference fallback is skipped
        result!.PreviousPayment.Should().Be(600m);
    }

    [Test]
    public async Task CalculateFees_WithInvalidRegulator_Returns400()
    {
        // Arrange
        var request = new ComplianceSchemeFeesRequestDto
        {
            Regulator = "INVALID",
            ApplicationReferenceNumber = "REF-CS-005",
            SubmissionDate = ValidSubmissionDate
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private static ComplianceSchemeFeesRequestDto BuildRequest(string reference, Guid? fileId = null) =>
        new()
        {
            Regulator = "GB-ENG",
            ApplicationReferenceNumber = reference,
            SubmissionDate = ValidSubmissionDate,
            FileId = fileId,
            IncludeRegistrationFee = true,
            ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
            {
                new()
                {
                    MemberId = "member-1",
                    MemberType = "Large",
                    IsOnlineMarketplace = false,
                    IsLateFeeApplicable = false,
                    NumberOfSubsidiaries = 0,
                    NoOfSubsidiariesOnlineMarketplace = 0
                }
            }
        };
}
