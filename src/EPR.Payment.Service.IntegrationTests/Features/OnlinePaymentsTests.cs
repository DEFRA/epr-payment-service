using System.Net;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.IntegrationTests.Features;

public class OnlinePaymentsTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GIVEN_OnlinePaymentExists_WHEN_GetOnlinePayment_THEN_Returns200WithOnlinePayment()
    {
        var payment = await Builder.Payment().Build();

        var response = await Client.GetAsync($"/api/v1/online-payments/{payment.ExternalPaymentId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.ReadJson<OnlinePaymentResponseDto>();
        payload.Should().BeEquivalentTo(new
        {
            payment.ExternalPaymentId,
            payment.UpdatedByUserId,
            payment.UpdatedByOrganisationId,
            payment.GovPayPaymentId,
            payment.Reference,
            payment.Amount,
            payment.Regulator,
            payment.Description,
            payment.RequestorType
        });
    }
}
