using System.Net;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationSubmission;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using EPR.Payment.Service.Messaging;

namespace EPR.Payment.Service.IntegrationTests.Features;

public class RegistrationSubmissionIntegrationTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GIVEN_Registration_file_uploaded_WHEN_RegistrationSubmittedMessage_consumed_THEN_fee_calculation_details_endpoint_returns_matching_data()
    {
        // Arrange
        var registrationFile = await Builder.RegistrationFileBuilder().Build();

        var message = new RegistrationSubmittedMessage(
            SubmissionId: registrationFile.SubmissionId,
            RegistrationBlobName: registrationFile.RegistrationBlobName,
            ComplianceSchemeId: null,
            SubmissionPeriod: "2024-P1",
            SubmissionDate: new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc));
        
        // act
        await SendMessageAndWaitUntilConsumed(message);

        // Assert
        var response = await Client.GetAsync($"/api/v1/registration-submission-data/{registrationFile.SubmissionId}/fee-calculation-details");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.ReadJson<List<RegistrationFeeCalculationDetailsDto>>();

        details.Should().HaveCount(1);
        var item = details[0];
        item.OrganisationId.Should().Be(registrationFile.BuiltRegistrationFileItems[0].OrganisationId);
        item.OrganisationSize.Should().Be("Large");  // L => Large
        item.IsOnlineMarketplace.Should().BeTrue();
        item.IsClosedLoopRecycling.Should().Be(registrationFile.BuiltRegistrationFileItems[0].ClosedLoopRegistration);
        item.IsNewJoiner.Should().BeTrue();
        item.NationId.Should().Be(1); // EN → 1
        item.NumberOfSubsidiaries.Should().Be(1);
        item.NumberOfSubsidiariesBeingOnlineMarketPlace.Should().Be(0);
        item.NumberOfSubsidiariesBeingClosedLoopRecycling.Should().Be(1);
    }

    [Fact]
    public async Task GIVEN_Registration_file_not_uploaded_and_submission_message_not_consumed_WHEN_fee_calculation_details_requested_THEN_endpoint_returns_404()
    {
        // Arrange
        var submissionId = Guid.NewGuid();
        
        // act
        var response = await Client.GetAsync($"/api/v1/registration-submission-data/{submissionId}/fee-calculation-details");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GIVEN_Registration_file_uploaded_and_submission_message_not_consumed_WHEN_fee_calculation_details_requested_THEN_endpoint_returns_404()
    {
        // Arrange
        var registrationFile = await Builder.RegistrationFileBuilder().Build();
        
        // act
        var response = await Client.GetAsync($"/api/v1/registration-submission-data/{registrationFile.SubmissionId}/fee-calculation-details");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GIVEN_two_message_received_in_order_WHEN_RegistrationSubmittedMessage_consumed_THEN_second_message_generates_fees()
    {
        // Arrange
        var earlierSubmissionDateRegistrationFile = await Builder.RegistrationFileBuilder().Build();
        var laterSubmissionDateRegistrationFile = await Builder.RegistrationFileBuilder().Build(earlierSubmissionDateRegistrationFile.SubmissionId);

        // the submission date determines the order. the later submission date should be the one that is used to generate fees
        var earlierSubmissionDateMessage = new RegistrationSubmittedMessage(
            SubmissionId: earlierSubmissionDateRegistrationFile.SubmissionId,
            RegistrationBlobName: earlierSubmissionDateRegistrationFile.RegistrationBlobName,
            ComplianceSchemeId: null,
            SubmissionPeriod: "2024-P1",
            SubmissionDate: new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc));
        
        await SendMessageAndWaitUntilConsumed(earlierSubmissionDateMessage);

        var laterSubmissionDateMessage = new RegistrationSubmittedMessage(
            SubmissionId: laterSubmissionDateRegistrationFile.SubmissionId,
            RegistrationBlobName: laterSubmissionDateRegistrationFile.RegistrationBlobName,
            ComplianceSchemeId: null,
            SubmissionPeriod: "2024-P1",
            SubmissionDate: new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc));

        // act
        await SendMessageAndWaitUntilConsumed(laterSubmissionDateMessage);
        
        // Assert
        var response = await Client.GetAsync($"/api/v1/registration-submission-data/{laterSubmissionDateRegistrationFile.SubmissionId}/fee-calculation-details");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.ReadJson<List<RegistrationFeeCalculationDetailsDto>>();

        details.Should().HaveCount(1);
        var item = details[0];
        item.OrganisationId.Should().Be(laterSubmissionDateRegistrationFile.BuiltRegistrationFileItems[0].OrganisationId);
    }
    
    [Fact]
    public async Task GIVEN_two_message_received_in_wrong_order_WHEN_RegistrationSubmittedMessage_consumed_THEN_first_message_generates_fees()
    {
        // Arrange
        var earlierSubmissionDateRegistrationFile = await Builder.RegistrationFileBuilder().Build();
        var laterSubmissionDateRegistrationFile = await Builder.RegistrationFileBuilder().Build(earlierSubmissionDateRegistrationFile.SubmissionId);

        // the submission date determines the order. the later submission date should be the one that is used to generate fees
        var laterSubmissionDateMessage = new RegistrationSubmittedMessage(
            SubmissionId: laterSubmissionDateRegistrationFile.SubmissionId,
            RegistrationBlobName: laterSubmissionDateRegistrationFile.RegistrationBlobName,
            ComplianceSchemeId: null,
            SubmissionPeriod: "2024-P1",
            SubmissionDate: new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc));

        await SendMessageAndWaitUntilConsumed(laterSubmissionDateMessage);

        var earlierSubmissionDateMessage = new RegistrationSubmittedMessage(
            SubmissionId: earlierSubmissionDateRegistrationFile.SubmissionId,
            RegistrationBlobName: earlierSubmissionDateRegistrationFile.RegistrationBlobName,
            ComplianceSchemeId: null,
            SubmissionPeriod: "2024-P1",
            SubmissionDate: new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc));
        
        // act
        await SendMessageAndWaitUntilConsumed(earlierSubmissionDateMessage);
        
        // Assert
        var response = await Client.GetAsync($"/api/v1/registration-submission-data/{laterSubmissionDateRegistrationFile.SubmissionId}/fee-calculation-details");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.ReadJson<List<RegistrationFeeCalculationDetailsDto>>();

        details.Should().HaveCount(1);
        var item = details[0];
        item.OrganisationId.Should().Be(laterSubmissionDateRegistrationFile.BuiltRegistrationFileItems[0].OrganisationId);
    }
}
