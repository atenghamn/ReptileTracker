using System;
using System.Collections.Generic;
using System.Linq;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Shedding.Errors;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Shedding.Service;

public sealed class SheddingService(IGenericRepository<SheddingEvent> sheddingRepository) : ISheddingService
{

    public Result<SheddingEvent> AddSheddingEvent(SheddingEvent SheddingEvent)
    {
        try
        {
            sheddingRepository.Add(SheddingEvent);
            sheddingRepository.Save();
            return Result<SheddingEvent>.Success(SheddingEvent);
        }
        catch (Exception ex)
        {
            return Result<SheddingEvent>.Failure(SheddingErrors.CantSave);
        }
    }

    public Result<SheddingEvent> GetSheddingEventById(int sheddingEventId)
    {
        var entity = sheddingRepository.GetById(sheddingEventId);
        return entity == null
            ? Result<SheddingEvent>.Failure(SheddingErrors.NotFound)
            : Result<SheddingEvent>.Success(entity: entity);
    }

    public Result<SheddingEvent> DeleteSheddingEvent(int sheddingEventId)
    {
        try
        {
            var entity = GetSheddingEventById(sheddingEventId);
            if (entity.Data == null) return Result<SheddingEvent>.Failure(SheddingErrors.NotFound);
            sheddingRepository.Delete(entity.Data);
            sheddingRepository.Save();
            return Result<SheddingEvent>.Success();
        }
        catch (Exception ex)
        {
            return Result<SheddingEvent>.Failure(SheddingErrors.CantDelete);
        }
    }

    public Result<SheddingEvent> UpdateSheddingEvent(SheddingEvent sheddingEvent)
    {
        try
        {
            var entity = GetSheddingEventById(sheddingEvent.Id);
            if (entity.Data == null) return Result<SheddingEvent>.Failure(SheddingErrors.NotFound);
            sheddingRepository.Update(sheddingEvent);
            sheddingRepository.Save();
            return Result<SheddingEvent>.Success(sheddingEvent);
        }
        catch (Exception ex)
        {
            return Result<SheddingEvent>.Failure(SheddingErrors.CantUpdate);
        }
    }

    public Result<List<SheddingEvent>> GetSheddingEvents()
    {
        try
        {
            var entities = sheddingRepository.GetAll();
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