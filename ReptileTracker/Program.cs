using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.EntityFramework;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Shedding.Model;
using ReptileTracker.Shedding.Service;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReptileContext>();

builder.Services.AddScoped<IFeedingService, FeedingService>();
builder.Services.AddScoped(typeof(IGenericRepository<FeedingEvent>), typeof(GenericRepository<FeedingEvent>));
builder.Services.AddScoped<ISheddingService, SheddingService > ();
builder.Services.AddScoped(typeof(IGenericRepository<SheddingEvent>), typeof(GenericRepository<SheddingEvent>));
builder.Services.AddScoped<ILengthService, LengthService>();
builder.Services.AddScoped<IWeightService, WeightService>();
builder.Services.AddScoped<IReptileService, ReptileService>();
builder.Services.AddScoped(typeof(IGenericRepository<Weight>), typeof(GenericRepository<Weight>));
builder.Services.AddScoped(typeof(IGenericRepository<Length>), typeof(GenericRepository<Length>));
builder.Services.AddScoped(typeof(IGenericRepository<Reptile>), typeof(GenericRepository<Reptile>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReptileTracker API");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// TODO: Remove this shit later, only for testing
app.MapPost("reptile/feeding", (FeedingEvent feedingEvent, IFeedingService feedingService) =>
{
    var result = feedingService.AddFeedingEvent(feedingEvent);

    return result.IsFailure ? Results.BadRequest() : Results.NoContent();
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

app.Run();