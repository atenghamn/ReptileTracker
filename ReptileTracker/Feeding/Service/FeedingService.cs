using System;
using System.Collections.Generic;
using System.Linq;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;
using Serilog.Core;

namespace ReptileTracker.Feeding.Service;

public sealed class FeedingService(IFeedingRepository feedingRepository) : IFeedingService
{
    public Result<FeedingEvent> GetFeedingEventById(int feedingId)
    {
        var entity = feedingRepository.GetById(feedingId);
        return entity == null
            ? Result<FeedingEvent>.Failure(FeedingErrors.NotFound)
            : Result<FeedingEvent>.Success(entity);
    }

    public Result<FeedingEvent> AddFeedingEvent(FeedingEvent feedingEvent)
    {
        try
        {
            feedingRepository.Add(feedingEvent);
            feedingRepository.Save();
            Log.Logger.Debug("Added new feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Success(feedingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Failure(FeedingErrors.CantSave);
        }
    }

    public Result<FeedingEvent> DeleteFeedingEvent(int id)
    {
        try
        {
            var entity = GetFeedingEventById(id);
            if (entity.Data == null) return Result<FeedingEvent>.Failure(FeedingErrors.NotFound);
            feedingRepository.Delete(entity.Data);
            feedingRepository.Save();
            Log.Logger.Debug("Deleted feeding event with id {FeedingId}", id);
            return Result<FeedingEvent>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to delete feeding event with id {FeedingId}", id);
            return Result<FeedingEvent>.Failure(FeedingErrors.CantDelete);
        }
    }

    public Result<FeedingEvent> UpdateFeedingEvent(FeedingEvent feedingEvent)
    {
        try
        {
            var entity = GetFeedingEventById(feedingEvent.Id);
            if (entity.Data == null) return Result<FeedingEvent>.Failure(FeedingErrors.NotFound);
            feedingRepository.Update(feedingEvent);
            feedingRepository.Save();
            Log.Logger.Debug("Updated feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Success(feedingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update event for for reptile");
            return Result<FeedingEvent>.Failure(FeedingErrors.CantUpdate);
        }
    }

    public Result<List<FeedingEvent>> GetFeedingEvents(int repitleId)
    {
        try
        {
            var entities = feedingRepository.GetAllForReptile(repitleId).Result;
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<FeedingEvent>>.Failure(FeedingErrors.NoFeedingHistory) : Result<List<FeedingEvent>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<FeedingEvent>>.Failure(FeedingErrors.EventlistNotFound);
        }
    }
}