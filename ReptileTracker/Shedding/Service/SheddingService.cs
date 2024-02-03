using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Shedding.Errors;
using ReptileTracker.Shedding.Model;
using Serilog;

namespace ReptileTracker.Shedding.Service;

public sealed class SheddingService(ISheddingRepository sheddingRepository) : ISheddingService
{

    public async Task<Result<SheddingEvent>> AddSheddingEvent(SheddingEvent sheddingEvent, CancellationToken ct)
    {
        try
        {
            await sheddingRepository.AddAsync(sheddingEvent, ct);
            await sheddingRepository.SaveAsync(ct);
            Log.Logger.Debug("Added shedding event to reptile {ReptileId}",sheddingEvent.ReptileId);
            return Result<SheddingEvent>.Success(sheddingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add shedding event to reptile {ReptileId}", sheddingEvent.ReptileId);
            return Result<SheddingEvent>.Failure(SheddingErrors.CantSave);
        }
    }

    public async Task<Result<SheddingEvent>> GetSheddingEventById(int sheddingEventId, CancellationToken ct)
    {
        var entity = await sheddingRepository.GetByIdAsync(sheddingEventId, ct);
        return entity == null
            ? Result<SheddingEvent>.Failure(SheddingErrors.NotFound)
            : Result<SheddingEvent>.Success(entity: entity);
    }

    public async Task<Result<SheddingEvent>> DeleteSheddingEvent(int sheddingEventId, CancellationToken ct)
    {
        try
        {
            var entity = await GetSheddingEventById(sheddingEventId, ct);
            if (entity.Data == null) return Result<SheddingEvent>.Failure(SheddingErrors.NotFound);
            sheddingRepository.Delete(entity.Data);
            await sheddingRepository.SaveAsync(ct);
            Log.Logger.Debug("Deleted event with id {Id}", sheddingEventId);
            return Result<SheddingEvent>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to delete event with id {Id}", sheddingEventId);
            return Result<SheddingEvent>.Failure(SheddingErrors.CantDelete);
        }
    }

    public async Task<Result<SheddingEvent>> UpdateSheddingEvent(SheddingEvent sheddingEvent, CancellationToken ct)
    {
        try
        {
            var entity = await GetSheddingEventById(sheddingEvent.Id, ct);
            if (entity.Data == null) return Result<SheddingEvent>.Failure(SheddingErrors.NotFound);
            sheddingRepository.Update(sheddingEvent);
            await sheddingRepository.SaveAsync(ct);
            Log.Logger.Debug("Updated shedding event {Id}", sheddingEvent.Id);
            return Result<SheddingEvent>.Success(sheddingEvent);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update shedding event  {Id}", sheddingEvent.Id);
            return Result<SheddingEvent>.Failure(SheddingErrors.CantUpdate);
        }
    }

    public async Task<Result<List<SheddingEvent>>> GetSheddingEvents(int reptileId, CancellationToken ct)
    {
        try
        {
            var entities = await sheddingRepository.GetAllForReptile(reptileId, ct);
            var list = entities.ToList();
            return list.Count < 1
                ? Result<List<SheddingEvent>>.Failure(SheddingErrors.EventlistNotFound)
                : Result<List<SheddingEvent>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<SheddingEvent>>.Failure(SheddingErrors.EventlistNotFound);
        }
    }
}