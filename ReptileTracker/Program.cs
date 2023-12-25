using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReptileTracker.Commons;
using ReptileTracker.EntityFramework;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Infrastructure.Persistence;

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

    return result.Match(
        onSuccess: Results.NoContent, 
        onFailure: Results.BadRequest);
});

app.Run();