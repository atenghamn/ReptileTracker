using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using Serilog;

namespace ReptileTracker.Animal.Service;

public sealed class ReptileService(IReptileRepository reptileRepository, IAccountRepository accountRepository) : IReptileService
{
    public async Task<Result<Reptile>> CreateReptile(string name, string species, DateTime birthdate, ReptileType type, string username, CancellationToken ct)
    {
        try
        {
            var account = await accountRepository.GetByUsername(username, ct);
            var accountId = account.Id;
            var reptile = new Reptile()
            {
                Name = name,
                Species = species,
                Birthdate = birthdate,
                ReptileType = type,
                AccountId = accountId
            };
            await reptileRepository.AddAsync(reptile, ct);
            await reptileRepository.SaveAsync(ct);
            
            Log.Logger.Debug("Added reptile {Name} to account {AccountId} ", name, accountId);
            return Result<Reptile>.Success(reptile);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to add reptile {Name} ", name);

            return Result<Reptile>.Failure(ReptileErrors.CantSave);
        }
    }

    public async Task<Result<Reptile>> UpdateReptile(Reptile entity, CancellationToken ct)
    {
        try
        {
            var newEntity = await reptileRepository.GetByIdAsync(entity.Id, ct);
            if (newEntity == null) return Result<Reptile>.Failure(ReptileErrors.NotFound);
            reptileRepository.Update(newEntity);
            await reptileRepository.SaveAsync(ct);
            Log.Logger.Debug("Updated reptile {Name} to account {AccountId} ",newEntity.Name, newEntity.Id);

            return Result<Reptile>.Success(newEntity);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failed to update reptile {Name} to account", entity.Name);

            return Result<Reptile>.Failure(ReptileErrors.CantUpdate);
        }
    }

    public async Task<Result<Reptile>> GetReptileById(int id, CancellationToken ct)
    {
        var entity = await reptileRepository.GetByIdAsync(id, ct);
        return entity == null
            ? Result<Reptile>.Failure(ReptileErrors.NotFound)
            : Result<Reptile>.Success(entity);
    }

    public async Task<Result<Reptile>> DeleteReptile(int id, CancellationToken ct)
    {
        try
        {
            var entity = await GetReptileById(id, ct);
            if (entity.Data == null) return Result<Reptile>.Failure(ReptileErrors.NotFound);
            reptileRepository.Delete(entity.Data);
            await reptileRepository.SaveAsync(ct);
            Log.Logger.Debug("Removed reptile {ReptileId} ", id);

            return Result<Reptile>.Success();
        }
        catch (Exception ex)
        {
            Log.Logger.Debug("Failed to remove reptile {ReptileId} ", id);

            return Result<Reptile>.Failure(ReptileErrors.CantDelete);
        }
    }

    public async Task<Result<List<Reptile?>>> GetReptilesByAccount(string username, CancellationToken ct)
    {
        try
        {
            var account = await accountRepository.GetByUsername(username, ct);
            var reptiles = await reptileRepository.GetByAccount(account.Id, ct);
            return reptiles is not null
                ? Result<List<Reptile?>>.Success(reptiles.ToList())
                : Result<List<Reptile?>>.Failure(ReptileErrors.DidntFindReptiles);
        }
        catch (Exception ex)
        {
            return Result<List<Reptile?>>.Failure(ReptileErrors.NotFound);
        }
    }
}