using eventify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using eventify.Application;
using Microsoft.OpenApi.Models;
using eventify.Infrastructure.Persistence.Repositories;
using eventify.Application.Repositories;
using eventify.Application.Common;
using eventify.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using eventify.Infrastructure.Identity;
using eventify.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EventsDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IConceptRepository, ConceptRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberFollowRepository, MemberFollowRepository>();
builder.Services.AddScoped<ITimeTableSlotRepository, TimeTableSlotRepository>();
builder.Services.AddScoped<IArtistProfileRepository, ArtistProfileRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IApplicationMarker>()
    .AddClasses(classes => classes
        .Where(type => 
            type.Name.EndsWith("Handler") ||
            type.Name.EndsWith("QueryHandler") ||
            type.Name.EndsWith("CommandHandler")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

// Identity and Auth
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v0", new OpenApiInfo { Title = "Eventify API", Version = "v0" });
});

var app = builder.Build();

if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();

    try
    {
        Console.WriteLine("⏳ Applying EventsDbContext migrations...");
        var eventsDb = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        eventsDb.Database.Migrate();
        Console.WriteLine("✅ EventsDbContext migrations applied.");

        Console.WriteLine("⏳ Applying AppIdentityDbContext migrations...");
        var identityDb = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        identityDb.Database.Migrate();
        Console.WriteLine("✅ AppIdentityDbContext migrations applied.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Migration failed: " + ex);
        throw;
    }

    return;
}

if (args.Contains("--seedIdentity"))
{
    using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedRolesAndAdminUserAsync(services);
}
}

if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v0/swagger.json", "eventify.API v0");
        });
    }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
