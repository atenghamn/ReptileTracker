using System;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReptileTracker.Account.Model;
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

builder.Services.AddCustomRateLimiting(builder.Configuration);
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
builder.Services.AddScoped(typeof(IGenericRepository<Account>), typeof(GenericRepository<Account>));
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IWeightRepository, WeightRepository>();
builder.Services.AddScoped<ILengthRepository, LengthRepository>();
builder.Services.AddScoped<IReptileRepository, ReptileRepository>();


builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<Account>()
    .AddEntityFrameworkStores<ReptileContext>()
    .AddApiEndpoints();

builder.Services.IdentityExtensions();
// builder.Services.CookieExtensions();
builder.Services.CorsExtension();
builder.Services.RateLimitingLoggingExtensions();

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

app.MapIdentityApi<Account>();

app.UseHttpsRedirection();
app.UseHttpLogging();
await app.UseCustomClientRateLimiting();

app.UseAuthorization();
app.UseCors("ReptileTrackerDashboard");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

app.MapGets();
app.MapPosts();
app.MapPuts();
app.MapDeletes();

// app.UseCors(policyName: "SomeExamplePolicy");

app.Run();