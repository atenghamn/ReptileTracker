using System;
using System.Security.Claims;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Shedding.Model;
using ReptileTracker.Shedding.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading;

namespace ReptileTracker.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapGets(this WebApplication app, int pageSize = 10)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
       
        app.MapGet("api/v{version:apiVersion}/reptile/shedding/{sheddingEventId:int}", async (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int sheddingEventId,
            CancellationToken ct
            ) =>
        {
            var result = await sheddingService.GetSheddingEventById(sheddingEventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("shedding"));

        app.MapGet("api/v{version:apiVersion}/reptile/shedding/list/{reptileId:int}", async (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int reptileId,
            CancellationToken ct
            ) =>
        {
            var result = await sheddingService.GetSheddingEvents(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("shedding"));

        app.MapGet("api/v{version:apiVersion}/reptile/feeding/{eventId:int}", async (
            [FromServices] FeedingService feedingService,
            [FromRoute] int eventId,
            CancellationToken ct) =>
        {
            var result = feedingService.GetFeedingEventById(eventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("feeding"));

        app.MapGet("api/v{version:apiVersion}/reptile/feeding/list/{reptileId:int}", (
            [FromServices] FeedingService feedingService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = feedingService.GetFeedingEvents(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("feeding"));

        app.MapGet("api/v{version:apiVersion}/reptile/weight/{eventId:int}", (
            [FromServices] WeightService weightService,
            [FromRoute] int eventId) =>
        {
            var result = weightService.GetWeightById(eventId);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("weight"));

        app.MapGet("api/v{version:apiVersion}/reptile/weight/list/{reptileId:int}", (
            [FromServices] WeightService weigthService,
            [FromRoute] int reptileId) =>
        {
            var result = weigthService.GetWeights(reptileId);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("weight"));
        
        app.MapGet("api/v{version:apiVersion}/reptile/length/{eventId:int}", async (
            [FromServices] LengthService lengthService,
            [FromRoute] int eventId,
            CancellationToken ct) =>
        {
            var result = await lengthService.GetLengthById(eventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("length"));

        app.MapGet("api/v{version:apiVersion}/reptile/length/list/{reptileId:int}", async (
            [FromServices] LengthService lengthService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = await lengthService.GetLengths(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("length"));

        app.MapGet("api/v{version:apiVersion}/reptile/{reptileId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int reptileId) =>
        {
            var result = reptileService.GetReptileById(reptileId);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("reptile"));

        app.MapGet("api/v{version:apiVersion}/reptile/list/{username}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] string username) =>
        {
            var result = reptileService.GetReptilesByAccount(username);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("reptile"));
        
        return app;
    }


    public static WebApplication MapPosts(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapPost("api/v{version:apiVersion}/reptile/feeding", async (
            FeedingEvent feedingEvent, 
            [FromServices]FeedingService feedingService, 
            IOutputCacheStore cache, 
            CancellationToken ct
            ) =>
        {
            var result = feedingService.AddFeedingEvent(feedingEvent, ct);
            await cache.EvictByTagAsync("feeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile/shedding", async (
            SheddingEvent sheddingEvent,
            [FromServices] SheddingService sheddingService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = sheddingService.AddSheddingEvent(sheddingEvent, ct);
            await cache.EvictByTagAsync("sheeding",ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile/length", async (
            Length lengthMeasurement,
            [FromServices] LengthService lengthService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = await lengthService.AddLength(lengthMeasurement, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();
        
        app.MapPost("api/v{version:apiVersion}/reptile/weight", async (
            Weight weigthMeasurement, 
            [FromServices] WeightService weightService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = weightService.AddWeight(weigthMeasurement);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile", async (
            string name, 
            string species, 
            DateTime birthdate, 
            ReptileType type, 
            ClaimsPrincipal user,
            [FromServices] ReptileService reptileService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = reptileService.CreateReptile(
                name: name,
                species: species,
                birthdate: birthdate,
                type: type,
                username: user.Identity.Name);

            await cache.EvictByTagAsync("reptile", ct);

            return result;
        }).RequireAuthorization();

        return app;
    }
    public static WebApplication MapPuts(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapPut("api/v{version:apiVersion}/reptile/feeding", async (
            FeedingEvent feedingEvent, 
            [FromServices]FeedingService feedingService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = feedingService.UpdateFeedingEvent(feedingEvent, ct);
            await cache.EvictByTagAsync("feeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile/shedding", async (
            SheddingEvent sheddingEvent,
            [FromServices] SheddingService sheddingService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = await sheddingService.UpdateSheddingEvent(sheddingEvent, ct);
            await cache.EvictByTagAsync("sheeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile/length", async (
            Length lengthMeasurement,
            [FromServices] LengthService lengthService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = await  lengthService.UpdateLength(lengthMeasurement, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();
        
        app.MapPut("api/v{version:apiVersion}/reptile/weight", async (
            Weight weightMeasurement, 
            [FromServices] WeightService weightService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = weightService.UpdateWeight(weightMeasurement);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile", async (
            Reptile reptile,
            [FromServices] ReptileService reptileService, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = reptileService.UpdateReptile(reptile);
            await cache.EvictByTagAsync("reptile", ct);
            return result;
        }).RequireAuthorization();

        return app;
    }

    public static WebApplication MapDeletes(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapDelete("api/v{version:apiVersion}/reptile/feeding/{feedingId:int}", async (
                [FromServices] FeedingService feedingService,
                [FromRoute] int feedingId, 
                IOutputCacheStore cache, 
                CancellationToken ct) =>
            {
                var result = feedingService.DeleteFeedingEvent(feedingId, ct);
                await cache.EvictByTagAsync("feeding", ct);
                return result;
            }
        ).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/shedding/{sheddingId:int}", async (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int sheddingId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await sheddingService.DeleteSheddingEvent(sheddingId, ct);
            await cache.EvictByTagAsync("shedding", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/length/{lengthId:int}", async (
            [FromServices] LengthService lengthService,
            [FromRoute] int lengthId, 
            IOutputCacheStore cache, 
            CancellationToken ct) =>
        {
            var result = await lengthService.DeleteLength(lengthId, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/weight/{weightId:int}", async (
            [FromServices] WeightService weightService,
            [FromRoute] int weightId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = weightService.DeleteWeight(weightId);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/{reptileId:int}", async (
            [FromServices] ReptileService reptileService,
            [FromRoute] int reptileId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = reptileService.DeleteReptile(reptileId);
            await cache.EvictByTagAsync("reptile", ct);
            return result;
        }).RequireAuthorization();
        
        return app;
    }
    

    
}
