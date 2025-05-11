using eventify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using eventify.Application;
using Microsoft.OpenApi.Models;
using eventify.Infrastructure.Persistence.Repositories;
using eventify.Application.Repositories;
using eventify.Application.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.UseAuthorization();
app.MapControllers();

app.Run();
