using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using eventify.Infrastructure.Extensions;
using eventify.Application.Common.Interfaces;
using eventify.SharedKernel;
using eventify.Domain.ValueObjects;
using Moq;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Hosting;

namespace Tickets.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _postgresContainer;

    public CustomWebApplicationFactory(PostgreSqlContainer postgresContainer)
    {
        _postgresContainer = postgresContainer;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<EventsDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<EventsDbContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            // Mock payment service
            var paymentMock = new Mock<IPaymentService>();
            paymentMock
                .Setup(p => p.InitiatePaymentSession(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<decimal>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(Result.Success(
                    new InitiatePaymentSessionResponse(
                        "https://fake-checkout.url/session/123",
                        Guid.NewGuid().ToString()
                    )
                ));

            services.AddSingleton(paymentMock.Object);
        });
    }
}
