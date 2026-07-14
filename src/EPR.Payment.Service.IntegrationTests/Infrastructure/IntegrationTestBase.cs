using Azure.Messaging.ServiceBus.Administration;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

[Collection(PaymentServiceCollection.Name)]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTestBase
{
    private readonly ServiceFixture _fixture;
    protected HttpClient Client { get; }
    
    protected readonly ServiceBusAdministrationClient ServiceBusAdministrationClient;
    protected readonly IConfiguration Configuration;

    /// <summary>
    /// Fluent test-data entrypoint. See <see cref="TestBuilders"/> for the available builders —
    /// <c>Builder.Producer().Build()</c>, <c>Builder.Regulator().InNation(x).Build()</c>,
    /// <c>Builder.SchemeOperator().WithAdmin().Build()</c>, etc.
    /// </summary>
    protected TestBuilders Builder { get; }

    protected IntegrationTestBase(ServiceFixture fixture)
    {
        _fixture = fixture;
        Client = _fixture.CreateHttpClient();
        Builder = new TestBuilders(fixture);
        ServiceBusAdministrationClient = _fixture.SharedServices.GetRequiredService<ServiceBusAdministrationClient>();
        Configuration = _fixture.SharedServices.GetRequiredService<IConfiguration>();
    }

    protected async Task SeedPaymentAsync(string? registrationBlobName, decimal amount, string reference, Status status = Status.Success)
    {
        using var scope = _fixture.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var id = Guid.NewGuid();
        var payment = new Common.Data.DataModels.Payment
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            UpdatedByUserId = id,
            ReasonForPayment = "Integration test seed",
            Reference = reference,
            Regulator = "GB-ENG",
            Amount = amount,
            InternalStatusId = status,
            OnlinePayment = new Common.Data.DataModels.OnlinePayment
            {
                OrganisationId = id,
                UpdatedByOrgId = id,
                RequestorTypeId = 1,
                GovPayStatus = status == Status.Success ? "Success" : "Failed",
                GovPayPaymentId = id.ToString()
            }
        };

        ctx.Payment.Add(payment);

        if (!string.IsNullOrEmpty(registrationBlobName))
        {
            var exists = await ctx.RegistrationSubmissionData
                .AnyAsync(r => r.RegistrationBlobName == registrationBlobName);
            if (!exists)
            {
                ctx.RegistrationSubmissionData.Add(new RegistrationSubmissionData
                {
                    SubmissionId = Guid.NewGuid(),
                    RegistrationBlobName = registrationBlobName,
                    SubmissionPeriod = "2025",
                    SubmissionDate = DateTime.UtcNow,
                    CreatedDate = DateTimeOffset.UtcNow
                });
            }
        }

        await ctx.SaveChangesAsync();
    }
}
