using System.Net.Http.Json;
using eventify.Application.Common.Interfaces;
using eventify.SharedKernel;
using Microsoft.Extensions.Configuration;
namespace eventify.Infrastructure.Payment;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _initiatePath;

    public PaymentService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Payment:BaseUrl"]
                   ?? throw new ArgumentNullException("Payment:BaseUrl not configured");
        _initiatePath = configuration["Payment:InitiatePath"]
                        ?? throw new ArgumentNullException("Payment:InitiatePath not configured");
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
            IdempotencyKey = Guid.NewGuid().ToString()
        };

        var fullUrl = new Uri(new Uri(_baseUrl), _initiatePath);

        var response = await _httpClient.PostAsJsonAsync(fullUrl.ToString(), cmd);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return Result.Failure<InitiatePaymentSessionResponse>($"Payment service error: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<InitiatePaymentSessionResponse>();
        return result is null
            ? Result.Failure<InitiatePaymentSessionResponse>("Payment service returned empty response.")
            : Result.Success(result);
    }
}

