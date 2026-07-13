using System.Net;
using System.Net.Http.Json;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using AwesomeAssertions;

namespace EPR.Payment.Service.IntegrationTests.Controllers;

public class ComplianceSchemeFeesControllerTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    private static readonly DateTime ValidSubmissionDate = new(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_ReturnsPreviousPaymentByBlobName()
    {
        // Arrange - seed a successful payment whose reference equals the blob name (join key)
        var blobName = $"blob-cs-001-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 750m, reference: blobName);

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        result!.PreviousPayment.Should().Be(750m);
        result.OutstandingPayment.Should().Be(result.TotalFee - 750m);
        result.TotalFee.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_WhenNoMatchingPayment_FallsBackToReferencePayment()
    {
        // Arrange - payment exists on the reference but no RegistrationSubmissionData for the blob name
        var reference = $"REF-CS-002-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: null, amount: 400m, reference: reference);

        var unknownBlobName = $"unknown-blob-{Guid.NewGuid():N}";
        var request = BuildRequest(applicationReferenceNumber: reference, registrationBlobName: unknownBlobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        // Blob name lookup returns 0 → falls back to reference → finds 400
        result!.PreviousPayment.Should().Be(400m);
    }

    [Fact]
    public async Task CalculateFees_WithoutRegistrationBlobName_UsesPreviousPaymentByReference()
    {
        // Arrange - payment only linked via reference, no blob name
        var reference = $"REF-CS-003-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: null, amount: 250m, reference: reference);

        var request = BuildRequest(applicationReferenceNumber: reference, registrationBlobName: null);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        result!.PreviousPayment.Should().Be(250m);
    }

    [Fact]
    public async Task CalculateFees_WithRegistrationBlobName_DoesNotDoubleCountWhenOtherPaymentsExist()
    {
        // Arrange - payment1 matches blob name; payment2 has a different reference
        var blobName = $"blob-cs-004-{Guid.NewGuid():N}";
        await SeedPaymentAsync(registrationBlobName: blobName, amount: 600m, reference: blobName);
        await SeedPaymentAsync(registrationBlobName: null, amount: 800m, reference: $"other-ref-{Guid.NewGuid():N}");

        var request = BuildRequest(applicationReferenceNumber: blobName, registrationBlobName: blobName);

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/compliance-scheme/registration-fee", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ComplianceSchemeFeesResponseDto>();
        // Blob name lookup returns 600 (non-zero), so reference fallback is skipped
        result!.PreviousPayment.Should().Be(600m);
    }

    [Fact]
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

    private static ComplianceSchemeFeesRequestDto BuildRequest(string applicationReferenceNumber, string? registrationBlobName = null) =>
        new()
        {
            Regulator = "GB-ENG",
            ApplicationReferenceNumber = applicationReferenceNumber,
            SubmissionDate = ValidSubmissionDate,
            RegistrationBlobName = registrationBlobName,
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
