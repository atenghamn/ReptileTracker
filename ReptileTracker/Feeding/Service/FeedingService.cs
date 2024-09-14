using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;

namespace ReptileTracker.Feeding.Service;

public sealed class FeedingService(IFeedingRepository feedingRepository) : IFeedingService
{
    public async Task<Result<FeedingEvent>> GetFeedingEventById(int feedingId, CancellationToken ct)
    {
        var entity = await feedingRepository.GetByIdAsync(feedingId, ct);
        return entity == null
            ? Result<FeedingEvent>.Failure(FeedingErrors.NotFound)
            : Result<FeedingEvent>.Success(entity);
    }

    public async Task<Result<FeedingEvent>> AddFeedingEvent(FeedingEvent feedingEvent, CancellationToken ct)
    {
        try
        {
            await feedingRepository.AddAsync(feedingEvent, ct);
            await feedingRepository.SaveAsync(ct);
            Log.Logger.Debug("Added new feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Success(feedingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Failure(FeedingErrors.CantSave);
        }
    }

    public async Task<Result<FeedingEvent>> DeleteFeedingEvent(int id, CancellationToken ct)
    {
        try
        {
            var entity = await GetFeedingEventById(id, ct);
            if (entity.Data == null) return Result<FeedingEvent>.Failure(FeedingErrors.NotFound);
            feedingRepository.Delete(entity.Data);
            await feedingRepository.SaveAsync(ct);
            Log.Logger.Debug("Deleted feeding event with id {FeedingId}", id);
            return Result<FeedingEvent>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to delete feeding event with id {FeedingId}", id);
            return Result<FeedingEvent>.Failure(FeedingErrors.CantDelete);
        }
    }

    public async Task<Result<FeedingEvent>> UpdateFeedingEvent(FeedingEvent feedingEvent, CancellationToken ct)
    {
        try
        {
            var entity = await GetFeedingEventById(feedingEvent.Id, ct);
            if (entity.Data == null) return Result<FeedingEvent>.Failure(FeedingErrors.NotFound);
            feedingRepository.Update(feedingEvent);
            await feedingRepository.SaveAsync(ct);
            Log.Logger.Debug("Updated feeding event to reptile {ReptileId}", feedingEvent.ReptileId);
            return Result<FeedingEvent>.Success(feedingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update event for for reptile");
            return Result<FeedingEvent>.Failure(FeedingErrors.CantUpdate);
        }
    }

    public async Task<Result<List<FeedingEvent>>> GetFeedingEvents(int repitleId, CancellationToken ct)
    {
        try
        {
            var entities = await feedingRepository.GetAllForReptile(repitleId);
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<FeedingEvent>>.Failure(FeedingErrors.NoFeedingHistory) : Result<List<FeedingEvent>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<FeedingEvent>>.Failure(FeedingErrors.EventlistNotFound);
        }
    }
}