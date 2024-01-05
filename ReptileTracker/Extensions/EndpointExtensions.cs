using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Shedding.Model;
using ReptileTracker.Shedding.Service;

namespace ReptileTracker.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapGets(this WebApplication app, int pageSize = 10)
    {
        app.MapGet("reptile/shedding/{sheddingEventId:int}", (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int sheddingEventId) =>
        {
            var result = sheddingService.GetSheddingEventById(sheddingEventId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/shedding/list/{reptileId:int}", (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int reptileId
            ) =>
        {
            var result = sheddingService.GetSheddingEvents(reptileId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/feeding/{eventId:int}", (
            [FromServices] FeedingService feedingService,
            [FromRoute] int eventId) =>
        {
            var result = feedingService.GetFeedingEventById(eventId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/feeding/list/{reptileId:int}", (
            [FromServices] FeedingService feedingService,
            [FromRoute] int reptileId) =>
        {
            var result = feedingService.GetFeedingEvents(reptileId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/weight/{eventId:int}", (
            [FromServices] WeightService weightService,
            [FromRoute] int eventId) =>
        {
            var result = weightService.GetWeightById(eventId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/weight/list/{reptileId:int}", (
            [FromServices] WeightService weigthService,
            [FromRoute] int reptileId) =>
        {
            var result = weigthService.GetWeights(reptileId);
            return result;
        }).RequireAuthorization();
        
        app.MapGet("reptile/length/{eventId:int}", (
            [FromServices] LengthService lengthService,
            [FromRoute] int eventId) =>
        {
            var result = lengthService.GetLengthById(eventId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/length/list/{reptileId:int}", (
            [FromServices] LengthService lengthService,
            [FromRoute] int reptileId) =>
        {
            var result = lengthService.GetLengths(reptileId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/{reptileId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int reptileId) =>
        {
            var result = reptileService.GetReptileById(reptileId);
            return result;
        }).RequireAuthorization();

        app.MapGet("reptile/list/{accountId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int accountId) =>
        {
            var result = reptileService.GetReptilesByAccount(accountId);
            return result;
        }).RequireAuthorization();
        
        return app;
    }


    public static WebApplication MapPosts(this WebApplication app)
    {
        app.MapPost("reptile/feeding", (FeedingEvent feedingEvent, 
            [FromServices]FeedingService feedingService) =>
        {
            var result = feedingService.AddFeedingEvent(feedingEvent);

            return result;
        }).RequireAuthorization();

        app.MapPost("reptile/shedding", (SheddingEvent sheddingEvent,
            [FromServices] SheddingService sheddingService) =>
        {
            var result = sheddingService.AddSheddingEvent(sheddingEvent);
            return result;
        }).RequireAuthorization();

        app.MapPost("reptile/length", (Length lengthMeasurement,
            [FromServices] LengthService lengthService) =>
        {
            var result = lengthService.AddLength(lengthMeasurement);
            return result;
        }).RequireAuthorization();
        
        app.MapPost("reptile/weight", (Weight weigthMeasurement, 
            [FromServices] WeightService weightService) =>
        {
            var result = weightService.AddWeight(weigthMeasurement);
            return result;
        }).RequireAuthorization();

        app.MapPost("reptile", (string name, string species, DateTime birthdate, ReptileType type, int accountId,
            [FromServices] ReptileService reptileService) =>
        {
            var result = reptileService.CreateReptile(
                name: name,
                species: species,
                birthdate: birthdate,
                type: type,
                accountId: accountId);

            return result;
        }).RequireAuthorization();

        return app;
    }
    public static WebApplication MapPuts(this WebApplication app)
    {
        app.MapPut("reptile/feeding", (FeedingEvent feedingEvent, 
            [FromServices]FeedingService feedingService) =>
        {
            var result = feedingService.UpdateFeedingEvent(feedingEvent);
            return result;
        }).RequireAuthorization();

        app.MapPut("reptile/shedding", (SheddingEvent sheddingEvent,
            [FromServices] SheddingService sheddingService) =>
        {
            var result = sheddingService.UpdateSheddingEvent(sheddingEvent);
            return result;
        }).RequireAuthorization();

        app.MapPut("reptile/length", (Length lengthMeasurement,
            [FromServices] LengthService lengthService) =>
        {
            var result = lengthService.UpdateLength(lengthMeasurement);
            return result;
        }).RequireAuthorization();
        
        app.MapPut("reptile/weight", (Weight weightMeasurement, 
            [FromServices] WeightService weightService) =>
        {
            var result = weightService.UpdateWeight(weightMeasurement);
            return result;
        }).RequireAuthorization();

        app.MapPut("reptile", (Reptile reptile,
            [FromServices] ReptileService reptileService) =>
        {
            var result = reptileService.UpdateReptile(reptile);
            return result;
        }).RequireAuthorization();

        return app;
    }

    public static WebApplication MapDeletes(this WebApplication app)
    {
        app.MapDelete("reptile/feeding/{feedingId:int}", (
                [FromServices] FeedingService feedingService,
                [FromRoute] int feedingId) =>
            {
                var result = feedingService.DeleteFeedingEvent(feedingId);
                return result;
            }
        ).RequireAuthorization();

        app.MapDelete("reptile/shedding/{sheddingId:int}", (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int sheddingId) =>
        {
            var result = sheddingService.DeleteSheddingEvent(sheddingId);
            return result;
        }).RequireAuthorization();

        app.MapDelete("reptile/length/{lengthId:int}", ([FromServices] LengthService lengthService,
            [FromRoute] int lengthId) =>
        {
            var result = lengthService.DeleteLength(lengthId);
            return result;
        }).RequireAuthorization();

        app.MapDelete("reptile/weight/{weightId:int}", (
            [FromServices] WeightService weightService,
            [FromRoute] int weightId) =>
        {
            var result = weightService.DeleteWeight(weightId);
            return result;
        }).RequireAuthorization();

        app.MapDelete("reptile/{reptileId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int reptileId) =>
        {
            var result = reptileService.DeleteReptile(reptileId);
            return result;
        }).RequireAuthorization();
        
        return app;
    }
    

    
}
