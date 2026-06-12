using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

// Copied from EPR.Payment.Service.Data.IntegrationTests/DatabaseDataGenerator.cs.
// Kept independent because the two test projects deliberately don't reference each other.
public static class DatabaseDataGenerator
{
    public static async Task<Common.Data.DataModels.Payment> InsertRandomPayment(AppDbContext context)
    {
        var id = Guid.NewGuid();
        var payment = new Common.Data.DataModels.Payment();
        payment.CreatedDate = DateTime.UtcNow;
        payment.UpdatedDate = payment.CreatedDate;
        payment.UpdatedByUserId = id;
        OnlinePayment onlinePayment = new()
        {
            OrganisationId = id,
            UpdatedByOrgId = id,
            RequestorTypeId = 1,
            GovPayStatus = "Success",
            GovPayPaymentId = id.ToString()
        };
        payment.OnlinePayment = onlinePayment;
        payment.OnlinePayment.UpdatedByOrgId = payment.OnlinePayment.OrganisationId;
        payment.ReasonForPayment = "foo";
        payment.Reference = "foobar";
        payment.Regulator = "GB-ENG";
        payment.Amount = 100;
        payment.InternalStatusId = Status.Success;

        context.Payment.Add(payment); 
        await context.SaveChangesAsync();

        return payment;
    }
}
