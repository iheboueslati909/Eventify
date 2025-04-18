using eventify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using eventify.Infrastructure;
using eventify.Application;
using Microsoft.OpenApi.Models;
using eventify.Infrastructure.Persistence.Repositories;
using eventify.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IConceptRepository, ConceptRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IArtistProfileRepository, ArtistProfileRepository>();
builder.Services.AddScoped<IMemberFollowRepository, MemberFollowRepository>();

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
