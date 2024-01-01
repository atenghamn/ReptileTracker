using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;

namespace ReptileTracker.Animal.Service;

public sealed class ReptileService(IReptileRepository reptileRepository) : IReptileService
{
    public Result<Reptile> CreateReptile(string name, string species, DateTime birthdate, ReptileType type, int accountId)
    {
        try
        {
            var reptile = new Reptile()
            {
                Name = name,
                Species = species,
                Birthdate = birthdate,
                ReptileType = type,
                AccountId = accountId
            };
            reptileRepository.Add(reptile);
            reptileRepository.Save();
            
            Log.Logger.Debug("Added reptile {Name} to account {AccountId} ", name, accountId);
            return Result<Reptile>.Success(reptile);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add reptile {Name} to account {AccountId} ", name, accountId);

            return Result<Reptile>.Failure(ReptileErrors.CantSave);
        }
    }

    public Result<Reptile> UpdateReptile(Reptile entity)
    {
        try
        {
            var newEntity = reptileRepository.GetById(entity.Id);
            if (newEntity == null) return Result<Reptile>.Failure(ReptileErrors.NotFound);
            reptileRepository.Update(newEntity);
            reptileRepository.Save();
            Log.Logger.Debug("Updated reptile {Name} to account {AccountId} ",newEntity.Name, newEntity.Id);

            return Result<Reptile>.Success(newEntity);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update reptile {Name} to account", entity.Name);

            return Result<Reptile>.Failure(ReptileErrors.CantUpdate);
        }
    }

    public Result<Reptile> GetReptileById(int id)
    {
        var entity = reptileRepository.GetById(id);
        return entity == null
            ? Result<Reptile>.Failure(ReptileErrors.NotFound)
            : Result<Reptile>.Success(entity);
    }

    public Result<Reptile> DeleteReptile(int id)
    {
        try
        {
            var entity = GetReptileById(id);
            if (entity.Data == null) return Result<Reptile>.Failure(ReptileErrors.NotFound);
            reptileRepository.Delete(entity.Data);
            reptileRepository.Save();
            Log.Logger.Debug("Removed reptile {ReptileId} ", id);

            return Result<Reptile>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Debug("Failed to remove reptile {ReptileId} ", id);

            return Result<Reptile>.Failure(ReptileErrors.CantDelete);
        }
    }

    public async Task<Result<List<Reptile>>> GetReptilesByAccount(int id)
    {
        try
        {
            var reptiles = await reptileRepository.GetByAccount(id);
            return reptiles is not null
                ? Result<List<Reptile>>.Success(reptiles.ToList())
                : Result<List<Reptile>>.Failure(ReptileErrors.DidntFindReptiles);
        }
        catch (Exception ex)
        {
            return Result<List<Reptile>>.Failure(ReptileErrors.NotFound);
        }
    }
}