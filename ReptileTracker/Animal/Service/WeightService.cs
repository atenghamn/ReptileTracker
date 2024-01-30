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

namespace ReptileTracker.Animal.Service;

public sealed class WeightService(IWeightRepository weightRepository) : IWeightService
{
    public async Task<Result<Weight>> AddWeight(Weight weight, CancellationToken ct)
    {
        try
        {
            await weightRepository.AddAsync(weight, ct);
            await weightRepository.SaveAsync(ct);
            Log.Logger.Debug("Added new weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Success(weight);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new weight measurement to reptile {WeightReptileId}", weight.ReptileId);

            return Result<Weight>.Failure(WeightErrors.CantSave);
        }
    }

    public async Task<Result<Weight>> GetWeightById(int weightId, CancellationToken ct)
    {
        var entity = await weightRepository.GetByIdAsync(weightId, ct);
        return entity == null
            ? Result<Weight>.Failure(WeightErrors.NotFound)
            : Result<Weight>.Success(entity);
    }

    public async Task<Result<Weight>> DeleteWeight(int weightId, CancellationToken ct)
    {
        try
        {
            var entity = await GetWeightById(weightId, ct);
            if (entity.Data == null) return Result<Weight>.Failure(WeightErrors.NotFound);
            weightRepository.Delete(entity.Data);
            await weightRepository.SaveAsync(ct);
            Log.Logger.Debug("Deleted weight measurement for reptile {WeightReptileId}", weightId);
            return Result<Weight>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to delete weight measurement for reptile {WeightReptileId}", weightId);
            return Result<Weight>.Failure(WeightErrors.CantDelete);
        }
    }

    public async Task<Result<Weight>> UpdateWeight(Weight weight, CancellationToken ct)
    {
        try
        {
            var entity = await GetWeightById(weight.Id, ct);
            if (entity.Data == null) return Result<Weight>.Failure(WeightErrors.NotFound);
            weightRepository.Update(weight);
            await weightRepository.SaveAsync(ct);
            Log.Logger.Debug("Failed to update weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Success(weight);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Failure(WeightErrors.CantUpdate);
        }
    }

    public async Task<Result<List<Weight>>> GetWeights(int reptileId, CancellationToken ct)
    {
        try
        {
            var entities = await weightRepository.GetAllForReptile(reptileId, ct);
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<Weight>>.Failure(WeightErrors.NoWeightHistory) : Result<List<Weight>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<Weight>>.Failure(WeightErrors.EventlistNotFound);
        }
    }
}