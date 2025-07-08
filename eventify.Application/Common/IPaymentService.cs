using eventify.SharedKernel;
namespace eventify.Application.Common.Interfaces;
public interface IPaymentService
{
    Task<Result<InitiatePaymentSessionResponse>> InitiatePaymentSession(Guid ticketId, Guid userId, decimal amount, string currency, string paymentMethod);
}

public record InitiatePaymentSessionResponse(string CheckoutUrl, string PaymentId);
