using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using PaymentEntity = EPR.Payment.Service.Common.Data.DataModels.Payment;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase
{
    protected HttpClient Client { get; private set; } = null!;

    [SetUp]
    public async Task SetUpAsync()
    {
        Client = ContainerFixture.Factory.CreateClient();

        await using var connection = new SqlConnection(ContainerFixture.ConnectionString);
        await connection.OpenAsync();
        await ContainerFixture.Respawner.ResetAsync(connection);
    }

    [TearDown]
    public void TearDown()
    {
        Client.Dispose();
    }

    protected async Task SeedPaymentAsync(Guid? fileId, decimal amount, string reference, Status status = Status.Success)
    {
        using var scope = ContainerFixture.Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Payment.Add(new PaymentEntity
        {
            UserId = Guid.NewGuid(),
            InternalStatusId = status,
            Regulator = "GB-ENG",
            Reference = reference,
            Amount = amount,
            ReasonForPayment = "Test payment",
            CreatedDate = DateTime.UtcNow,
            UpdatedByUserId = Guid.NewGuid(),
            UpdatedDate = DateTime.UtcNow,
            FileId = fileId
        });

        await context.SaveChangesAsync();
    }
}
