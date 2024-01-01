using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using ReptileTracker.Extensions;

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
builder.Services.AddScoped<IFeedingRepository, FeedingRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<SheddingEvent>), typeof(GenericRepository<SheddingEvent>));
builder.Services.AddScoped<ISheddingRepository, SheddingRepository>();
builder.Services.AddScoped<ISheddingService, SheddingService>();
builder.Services.AddScoped<ILengthService, LengthService>();
builder.Services.AddScoped<IWeightService, WeightService>();
builder.Services.AddScoped<IReptileService, ReptileService>();
builder.Services.AddScoped(typeof(IGenericRepository<Weight>), typeof(GenericRepository<Weight>));
builder.Services.AddScoped(typeof(IGenericRepository<Length>), typeof(GenericRepository<Length>));
builder.Services.AddScoped(typeof(IGenericRepository<Reptile>), typeof(GenericRepository<Reptile>));
builder.Services.AddScoped<IWeightRepository, WeightRepository>();
builder.Services.AddScoped<ILengthRepository, LengthRepository>();
builder.Services.AddScoped<IReptileRepository, ReptileRepository>();

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

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

app.MapGets();
app.MapPosts();
app.MapPuts();
app.MapDeletes();

app.Run();