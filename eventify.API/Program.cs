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

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EventsDbContext>(options =>
    options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure())
);
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure())
);


builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IConceptRepository, ConceptRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberFollowRepository, MemberFollowRepository>();

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
    
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v0", new OpenApiInfo { Title = "Eventify API", Version = "v0" });
});

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

// Place authentication middleware before authorization
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v0/swagger.json", "eventify.API v0");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();  // <-- Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();     // <-- Required for controllers to work
app.Run();               // <-- This keeps the app running