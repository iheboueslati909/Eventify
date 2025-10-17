using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using eventify.Infrastructure.Extensions;

namespace Tickets.IntegrationTests.Fixtures;

public class TestDatabaseFixture : IAsyncLifetime
{
    private static readonly object _lock = new();
    private static bool _initialized = false;

    public PostgreSqlContainer PostgresContainer { get; private set; } = default!;
    public CustomWebApplicationFactory Factory { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        lock (_lock)
        {
            if (_initialized)
                return;
            _initialized = true;
        }

        // 1. Start PostgreSQL container
        PostgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("eventify_test")
            .WithUsername("postgres")
            .WithPassword("123")
            .WithCleanUp(true)
            .Build();

        await PostgresContainer.StartAsync();

        // 2. Create factory
        Factory = new CustomWebApplicationFactory(PostgresContainer);

        // 3. Apply migrations
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        await db.Database.MigrateAsync();

        // 4. Clean database (safe for fresh DB)
        await CleanDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        // Don't stop container here, reuse it across test collection
        return Task.CompletedTask;
    }

    public async Task CleanDatabaseAsync()
{
    using var scope = Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EventsDbContext>();

    // Delete all rows from all DbSets dynamically
    var dbSets = db.Model.GetEntityTypes()
        .Select(t => t.GetTableName())
        .Where(t => !string.IsNullOrEmpty(t));

    foreach (var table in dbSets)
    {
        await db.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{table}\" RESTART IDENTITY CASCADE;");
    }
}

}
