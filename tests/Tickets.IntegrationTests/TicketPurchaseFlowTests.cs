using System.Net;
using System.Net.Http.Json;
using Xunit;
using Tickets.IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using eventify.Domain.Entities;
using eventify.Domain.Enums;
using eventify.Domain.ValueObjects;
using eventify.Application.Tickets.Commands;
using Microsoft.EntityFrameworkCore;
using eventify.SharedKernel;
using eventify.Infrastructure.Extensions;
using eventify.Domain.Common.Enums;

namespace Tickets.IntegrationTests.Tests;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<TestDatabaseFixture> { }

[Collection("IntegrationTestCollection")]
public class TicketPurchaseFlowTests : IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;

    public TicketPurchaseFlowTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.CleanDatabaseAsync();
        await SeedAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task SeedAsync()
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EventsDbContext>();

        var genres = MusicGenreCollection.Create(new[] { MusicGenre.Techno, MusicGenre.House }).Value;
        
        var user = Member.Create(Name.Create("Test").Value, Name.Create("User").Value, Email.Create("user@test.com").Value).Value;
        var concept = Concept.Create(user.Id, Title.Create("Concept").Value, Description.Create("Desc").Value, genres).Value;
        var artist = ArtistProfile.Create(user.Id, Name.Create("Artist").Value, Email.Create("a@test.com").Value, Bio.Create("Bio").Value, SocialMediaLinks.Create(["https://instagram.com/test"]).Value, genres).Value;

        var now = DateTime.UtcNow.AddHours(1);
        var timeTable = new List<(Title StageName,
        IEnumerable<(DateTime StartTime, DateTime EndTime, Title SlotTitle, IEnumerable<ArtistProfile> ArtistProfiles)> Slots)>
            { ( Title.Create("Main Stage").Value, new List<(DateTime, DateTime, Title, IEnumerable<ArtistProfile>)>
                {( now, now.AddHours(2), Title.Create("Slot 1 warmup").Value, new List<ArtistProfile> { artist } ), ( now.AddHours(2), now.AddHours(4), Title.Create("Slot 2 headline").Value, new List<ArtistProfile> { artist } ) } ) };

        var evnt = Event.Create(Title.Create("Test Event").Value, Description.Create("Event Desc").Value, now, now.AddHours(2), Location.Create("Test Location").Value, EventType.ClubNight, concept.Id, timeTable).Value;
        evnt.Publish();
        var ticket = Ticket.Create(evnt.Id, user.Id, 20, Name.Create("Test ticket").Value, 10, 0, Currency.USD).Value;
        
        await db.Members.AddAsync(user);
        await db.ArtistProfiles.AddAsync(artist);
        await db.Concepts.AddAsync(concept);
        await db.Events.AddAsync(evnt);
        await db.Tickets.AddAsync(ticket);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task Create_And_Fulfill_TicketPurchase_Flow_Succeeds()
    {
        var client = _fixture.Factory.CreateClient();

        using var scope = _fixture.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        var user = await db.Members.FirstAsync();
        var ticket = await db.Tickets.FirstAsync();

        var command = new CreateTicketPurchaseCommand(ticket.Id, user.Id, 2, null);
        var createResponse = await client.PostAsJsonAsync("/api/ticket-purchases", command);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var result = await createResponse.Content.ReadFromJsonAsync<CreateTicketPurchaseResult>();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.CheckoutUrl));

        var fulfillCommand = new FullFillTicketPurchaseCommand(result.TicketPurchaseId);
        var fulfillResponse = await client.PostAsJsonAsync($"/api/ticket-purchases/{result.TicketPurchaseId}/fulfill", fulfillCommand);
        Assert.Equal(HttpStatusCode.OK, fulfillResponse.StatusCode);

        var purchase = await db.TicketPurchases.FindAsync(result.TicketPurchaseId);
        Assert.NotNull(purchase);
        Assert.True(purchase.Status == TicketPurchaseStatus.Paid);
    }
}
