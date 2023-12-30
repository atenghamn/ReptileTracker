using System;
using System.Collections.Generic;
using System.Linq;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;

namespace ReptileTracker.Animal.Service;

public class WeightService(IGenericRepository<Weight> weightRepository) : IWeightService
{
    public Result<Weight> AddWeight(Weight weight)
    {
        try
        {
            weightRepository.Add(weight);
            weightRepository.Save();
            Log.Logger.Debug("Added new weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Success(weight);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add new weight measurement to reptile {WeightReptileId}", weight.ReptileId);

            return Result<Weight>.Failure(WeightErrors.CantSave);
        }
    }

    public Result<Weight> GetWeightById(int weightId)
    {
        var entity = weightRepository.GetById(weightId);
        return entity == null
            ? Result<Weight>.Failure(WeightErrors.NotFound)
            : Result<Weight>.Success(entity);
    }

    public Result<Weight> DeleteWeight(int weightId)
    {
        try
        {
            var entity = GetWeightById(weightId);
            if (entity.Data == null) return Result<Weight>.Failure(WeightErrors.NotFound);
            weightRepository.Delete(entity.Data);
            weightRepository.Save();
            Log.Logger.Debug("Deleted weight measurement for reptile {WeightReptileId}", weightId);
            return Result<Weight>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to delete weight measurement for reptile {WeightReptileId}", weightId);
            return Result<Weight>.Failure(WeightErrors.CantDelete);
        }
    }

    public Result<Weight> UpdateWeight(Weight weight)
    {
        try
        {
            var entity = GetWeightById(weight.Id);
            if (entity.Data == null) return Result<Weight>.Failure(WeightErrors.NotFound);
            weightRepository.Update(weight);
            weightRepository.Save();
            Log.Logger.Debug("Failed to update weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Success(weight);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update weight measurement to reptile {WeightReptileId}", weight.ReptileId);
            return Result<Weight>.Failure(WeightErrors.CantUpdate);
        }
    }

    public Result<List<Weight>> GetWeights()
    {
        try
        {
            var entities = weightRepository.GetAll();
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<Weight>>.Failure(WeightErrors.NoWeightHistory) : Result<List<Weight>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<Weight>>.Failure(WeightErrors.EventlistNotFound);
        }
    }
}