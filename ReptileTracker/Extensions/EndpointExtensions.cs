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
using ReptileTracker.Account.Service;

namespace ReptileTracker.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapGets(this WebApplication app, int pageSize = 10)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapGet("api/v{version:apiVersion}/account/info/{accountId}", async (
            [FromServices] IAccountService accountService,
            [FromRoute] string accountId,
            CancellationToken ct
            ) =>
        {
            var result = await accountService.GetAccountById(accountId, ct);
            return result;

        }).RequireAuthorization().CacheOutput(x => x.Tag("accountInfo"));

        app.MapGet("api/v{version:apiVersion}/reptile/shedding/{sheddingEventId:int}", async (
            [FromServices] ISheddingService sheddingService,
            [FromRoute] int sheddingEventId,
            CancellationToken ct
            ) =>
        {
            var result = await sheddingService.GetSheddingEventById(sheddingEventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("shedding"));

        app.MapGet("api/v{version:apiVersion}/reptile/shedding/list/{reptileId:int}", async (
            [FromServices] ISheddingService sheddingService,
            [FromRoute] int reptileId,
            CancellationToken ct
            ) =>
        {
            var result = await sheddingService.GetSheddingEvents(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("shedding"));

        app.MapGet("api/v{version:apiVersion}/reptile/feeding/{eventId:int}", async (
            [FromServices] IFeedingService feedingService,
            [FromRoute] int eventId,
            CancellationToken ct) =>
        {
            var result = await feedingService.GetFeedingEventById(eventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("feeding"));

        app.MapGet("api/v{version:apiVersion}/reptile/feeding/list/{reptileId:int}", (
            [FromServices] IFeedingService feedingService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = feedingService.GetFeedingEvents(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("feeding"));

        app.MapGet("api/v{version:apiVersion}/reptile/weight/{eventId:int}", async (
            [FromServices] IWeightService weightService,
            [FromRoute] int eventId,
            CancellationToken ct) =>
        {
            var result = await weightService.GetWeightById(eventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("weight"));

        app.MapGet("api/v{version:apiVersion}/reptile/weight/list/{reptileId:int}", async (
            [FromServices] IWeightService weigthService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = await weigthService.GetWeights(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("weight"));

        app.MapGet("api/v{version:apiVersion}/reptile/length/{eventId:int}", async (
            [FromServices] ILengthService lengthService,
            [FromRoute] int eventId,
            CancellationToken ct) =>
        {
            var result = await lengthService.GetLengthById(eventId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("length"));

        app.MapGet("api/v{version:apiVersion}/reptile/length/list/{reptileId:int}", async (
            [FromServices] ILengthService lengthService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = await lengthService.GetLengths(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("length"));

        app.MapGet("api/v{version:apiVersion}/reptile/{reptileId:int}", async (
            [FromServices] IReptileService reptileService,
            [FromRoute] int reptileId,
            CancellationToken ct) =>
        {
            var result = await reptileService.GetReptileById(reptileId, ct);
            return result;
        }).RequireAuthorization().CacheOutput(x => x.Tag("reptile"));

        app.MapGet("api/v{version:apiVersion}/reptile/list/{username}", async (
            [FromServices] IReptileService reptileService,
            [FromRoute] string username,
            CancellationToken ct) =>
        {
            var result = await reptileService.GetReptilesByAccount(username, ct);
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
            [FromServices] IFeedingService feedingService,
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
            [FromServices] ISheddingService sheddingService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = sheddingService.AddSheddingEvent(sheddingEvent, ct);
            await cache.EvictByTagAsync("sheeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile/length", async (
            Length lengthMeasurement,
            [FromServices] ILengthService lengthService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await lengthService.AddLength(lengthMeasurement, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile/weight", async (
            Weight weigthMeasurement,
            [FromServices] IWeightService weightService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await weightService.AddWeight(weigthMeasurement, ct);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapPost("api/v{version:apiVersion}/reptile", async (
            string name,
            string species,
            DateTime birthdate,
            ReptileType type,
            ClaimsPrincipal user,
            [FromServices] IReptileService reptileService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await reptileService.CreateReptile(
                name: name,
                species: species,
                birthdate: birthdate,
                type: type,
                username: user.Identity.Name,
                ct);

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
            [FromServices] IFeedingService feedingService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = feedingService.UpdateFeedingEvent(feedingEvent, ct);
            await cache.EvictByTagAsync("feeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile/shedding", async (
            SheddingEvent sheddingEvent,
            [FromServices] ISheddingService sheddingService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await sheddingService.UpdateSheddingEvent(sheddingEvent, ct);
            await cache.EvictByTagAsync("sheeding", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile/length", async (
            Length lengthMeasurement,
            [FromServices] ILengthService lengthService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await lengthService.UpdateLength(lengthMeasurement, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile/weight", async (
            Weight weightMeasurement,
            [FromServices] IWeightService weightService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await weightService.UpdateWeight(weightMeasurement, ct);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapPut("api/v{version:apiVersion}/reptile", async (
            Reptile reptile,
            [FromServices] IReptileService reptileService,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await reptileService.UpdateReptile(reptile, ct);
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
                [FromServices] IFeedingService feedingService,
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
            [FromServices] ISheddingService sheddingService,
            [FromRoute] int sheddingId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await sheddingService.DeleteSheddingEvent(sheddingId, ct);
            await cache.EvictByTagAsync("shedding", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/length/{lengthId:int}", async (
            [FromServices] ILengthService lengthService,
            [FromRoute] int lengthId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await lengthService.DeleteLength(lengthId, ct);
            await cache.EvictByTagAsync("length", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/weight/{weightId:int}", async (
            [FromServices] IWeightService weightService,
            [FromRoute] int weightId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await weightService.DeleteWeight(weightId, ct);
            await cache.EvictByTagAsync("weight", ct);
            return result;
        }).RequireAuthorization();

        app.MapDelete("api/v{version:apiVersion}/reptile/{reptileId:int}", async (
            [FromServices] IReptileService reptileService,
            [FromRoute] int reptileId,
            IOutputCacheStore cache,
            CancellationToken ct) =>
        {
            var result = await reptileService.DeleteReptile(reptileId, ct);
            await cache.EvictByTagAsync("reptile", ct);
            return result;
        }).RequireAuthorization();

        return app;
    }

}
