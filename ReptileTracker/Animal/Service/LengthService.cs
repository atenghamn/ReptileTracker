using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;
using Serilog.Core;

namespace ReptileTracker.Animal.Service;

public sealed class LengthService(ILengthRepository lengthRepository) : ILengthService
{
    public Result<Length> AddLength(Length length)
    {
        try
        {
            lengthRepository.Add(length);
            lengthRepository.Save();
            Log.Logger.Debug("Added new length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Failure(LengthErrors.CantSave);
        }
    }

    public Result<Length> GetLengthById(int lengthId)
    {
        var entity = lengthRepository.GetById(lengthId);
        return entity == null
            ? Result<Length>.Failure(LengthErrors.NotFound)
            : Result<Length>.Success(entity);
    }

    public Result<Length> DeleteLength(int lengthId)
    {
        try
        {
            var entity = GetLengthById(lengthId);
            if (entity.Data == null) return Result<Length>.Failure(LengthErrors.NotFound);
            lengthRepository.Delete(entity.Data);
            lengthRepository.Save();
            Log.Logger.Debug("Deleted length measurement to reptile {LengthReptileId}", lengthId);
            return Result<Length>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add  new length measurement to reptile {LengthReptileId}", lengthId);
            return Result<Length>.Failure(LengthErrors.CantDelete);
        }
    }

    public Result<Length> UpdateLength(Length length)
    {
        try
        {
            var entity = GetLengthById(length.Id);
            if (entity.Data == null) return Result<Length>.Failure(LengthErrors.NotFound);
            lengthRepository.Update(length);
            lengthRepository.Save();
            Log.Logger.Debug("Updated length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Failure(LengthErrors.CantUpdate);
        }
    }

    public async Task<Result<List<Length>>> GetLengths(int reptileId)
    {
        try
        {
            var entities = lengthRepository.GetAllForReptile(reptileId).Result;
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<Length>>.Failure(LengthErrors.NoLengthHistory) : Result<List<Length>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<Length>>.Failure(LengthErrors.EventlistNotFound);
        }
    }
}