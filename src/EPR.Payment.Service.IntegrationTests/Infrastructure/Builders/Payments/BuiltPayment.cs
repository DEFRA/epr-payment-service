namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Payments;

/// <summary>
/// Bundle returned by the <see cref="PaymentBuilder"/>
/// </summary>
public sealed record BuiltPayment(Common.Data.DataModels.Payment Payment)
{
    public int Id => Payment.Id;
    public Guid ExternalPaymentId { get; } = Payment.ExternalPaymentId;
    public Guid UpdatedByUserId { get; } = Payment.UpdatedByUserId;
    public Guid UpdatedByOrganisationId { get; } = Payment.OnlinePayment.UpdatedByOrgId;
    public string? GovPayPaymentId { get; } = Payment.OnlinePayment.GovPayPaymentId;
    public string? Reference { get; set; } = Payment.Reference;
    public decimal Amount { get; set; } = Payment.Amount;
    public string? Regulator { get; set; } = Payment.Regulator;
    public string? Description { get; set; } = Payment.ReasonForPayment;
    public string RequestorType { get; set; } = Payment.OnlinePayment.RequestorType.Type;
}
