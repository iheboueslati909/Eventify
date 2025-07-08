using System.Net.Http.Json;
using eventify.Application.Common.Interfaces;
using eventify.SharedKernel;
namespace eventify.Infrastructure.Payment;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<InitiatePaymentSessionResponse>> InitiatePaymentSession(
        Guid ticketId, Guid userId, decimal amount, string currency, string paymentMethod)
    {
        var cmd = new
        {
            Amount = amount,
            Currency = currency,
            PaymentMethod = paymentMethod,
            IntendId = ticketId,
            UserId = userId,
            IdempotencyKey = Guid.NewGuid().ToString() // or TicketPurchaseId for deterministic retries
        };

        var response = await _httpClient.PostAsJsonAsync("/api/payment/initiate-session", cmd);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return Result.Failure<InitiatePaymentSessionResponse>($"Payment service error: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<InitiatePaymentSessionResponse>();
        return Result.Success(result);
    }
}
