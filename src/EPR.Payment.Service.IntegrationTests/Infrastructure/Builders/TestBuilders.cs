using Azure.Storage.Blobs;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Payments;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Registrations;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

/// <summary>
/// Fluent entry point for integration-test data construction. Each factory method returns a
/// builder whose chained configuration calls refine the setup; <c>Build()</c> materialises the row.
/// Usage: <c>var payment = await Builder.Payment().Build();</c>
/// </summary>
public sealed class TestBuilders(ServiceFixture fixture)
{
    internal async Task WithDbContextAsync(Func<AppDbContext, Task> action, bool save)
    {
        using var scope = fixture.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await action(ctx);
        if (save) await ctx.SaveChangesAsync();
    }

    internal async Task WithBlobContainer(Func<BlobContainerClient, Task> action)
    {
        using var scope = fixture.CreateScope();
        var blobServiceClient = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
        var containerClient = blobServiceClient.GetBlobContainerClient(ServiceFixture.BlobContainerName);
        await containerClient.CreateIfNotExistsAsync();
        await action(containerClient);
    }

    public PaymentBuilder Payment() => new(this);
    
    public RegistrationFileBuilder RegistrationFileBuilder() => new(this);
}
