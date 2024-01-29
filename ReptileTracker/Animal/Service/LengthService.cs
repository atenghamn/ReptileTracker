using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public async Task<Result<Length>> AddLength(Length length, CancellationToken ct)
    {
        try
        {
            await lengthRepository.AddAsync(length, ct);
            await lengthRepository.SaveAsync(ct);
            Log.Logger.Debug("Added new length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Failure(LengthErrors.CantSave);
        }
    }

    public async Task<Result<Length>> GetLengthById(int lengthId, CancellationToken ct)
    {
        var entity = await lengthRepository.GetByIdAsync(lengthId, ct);
        return entity == null
            ? Result<Length>.Failure(LengthErrors.NotFound)
            : Result<Length>.Success(entity);
    }

    public async Task<Result<Length>> DeleteLength(int lengthId, CancellationToken ct)
    {
        try
        {
            var entity = await GetLengthById(lengthId, ct);
            if (entity.Data == null) return Result<Length>.Failure(LengthErrors.NotFound);
            lengthRepository.Delete(entity.Data);
            await lengthRepository.SaveAsync(ct);
            Log.Logger.Debug("Deleted length measurement to reptile {LengthReptileId}", lengthId);
            return Result<Length>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add  new length measurement to reptile {LengthReptileId}", lengthId);
            return Result<Length>.Failure(LengthErrors.CantDelete);
        }
    }

    public async Task<Result<Length>> UpdateLength(Length length, CancellationToken ct)
    {
        try
        {
            var entity = await GetLengthById(length.Id, ct);
            if (entity.Data == null) return Result<Length>.Failure(LengthErrors.NotFound);
            lengthRepository.Update(length);
            await lengthRepository.SaveAsync(ct);
            Log.Logger.Debug("Updated length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update length measurement to reptile {LengthReptileId}", length.ReptileId);
            return Result<Length>.Failure(LengthErrors.CantUpdate);
        }
    }

    public async Task<Result<List<Length>>> GetLengths(int reptileId, CancellationToken ct)
    {
        try
        {
            var entities = await lengthRepository.GetAllForReptile(reptileId);
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<Length>>.Failure(LengthErrors.NoLengthHistory) : Result<List<Length>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<Length>>.Failure(LengthErrors.EventlistNotFound);
        }
    }
}