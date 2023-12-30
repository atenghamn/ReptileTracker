using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using ReptileTracker.Animal.Service;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Shedding.Service;

namespace ReptileTracker.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapGet(this WebApplication app, int pageSize = 10)
    {
        app.MapGet("reptile/shedding/{sheddingEventId:int}", (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int sheddingEventId) =>
        {
            var result = sheddingService.GetSheddingEventById(sheddingEventId);
            return result;
        });

        app.MapGet("reptile/shedding/list/{reptileId:int}", (
            [FromServices] SheddingService sheddingService,
            [FromRoute] int reptileId
            ) =>
        {
            var result = sheddingService.GetSheddingEvents(reptileId);
            return result;
        });

        app.MapGet("reptile/feeding/{eventId:int}", (
            [FromServices] FeedingService feedingService,
            [FromRoute] int eventId) =>
        {
            var result = feedingService.GetFeedingEventById(eventId);
            return result;
        });

        app.MapGet("reptile/feeding/list/{reptileId:int}", (
            [FromServices] FeedingService feedingService,
            [FromRoute] int reptileId) =>
        {
            var result = feedingService.GetFeedingEvents(reptileId);
            return result;
        });

        app.MapGet("reptile/weight/{eventId:int}", (
            [FromServices] WeightService weightService,
            [FromRoute] int eventId) =>
        {
            var result = weightService.GetWeightById(eventId);
            return result;
        });

        app.MapGet("reptile/weight/list/{reptileId:int}", (
            [FromServices] WeightService weigthService,
            [FromRoute] int reptileId) =>
        {
            var result = weigthService.GetWeights(reptileId);
            return result;
        });
        
        app.MapGet("reptile/length/{eventId:int}", (
            [FromServices] LengthService lengthService,
            [FromRoute] int eventId) =>
        {
            var result = lengthService.GetLengthById(eventId);
            return result;
        });

        app.MapGet("reptile/length/list/{reptileId:int}", (
            [FromServices] LengthService lengthService,
            [FromRoute] int reptileId) =>
        {
            var result = lengthService.GetLengths(reptileId);
            return result;
        });

        app.MapGet("reptile/{reptileId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int reptileId) =>
        {
            var result = reptileService.GetReptileById(reptileId);
            return result;
        });

        app.MapGet("reptile/list/{accountId:int}", (
            [FromServices] ReptileService reptileService,
            [FromRoute] int accountId) =>
        {
            var result = reptileService.GetReptilesByAccount(accountId);
            return result;
        });
        
        return app;
    }
    /*
     *    app.MapPost("reptile/feeding", (FeedingEvent feedingEvent, 
                [FromServices]FeedingService feedingService) =>
            {
                var result = feedingService.AddFeedingEvent(feedingEvent);

                return result.IsFailure ? Results.BadRequest() : Results.NoContent();
            });
     */
    
}
