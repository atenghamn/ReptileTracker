using System;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTracker.Animal.Service;

public class ReptileService(IGenericRepository<Reptile> reptileRepository) : IReptileService
{
    public Result<Reptile> CreateReptile(string name, string species, DateTime birthdate, ReptileType type)
    {
        try
        {
            var reptile = new Reptile()
            {
                Name = name,
                Species = species,
                Birthdate = birthdate,
                ReptileType = type
            };
            reptileRepository.Add(reptile);
            reptileRepository.Save();
            return Result<Reptile>.Success(reptile);
        }
        catch (Exception ex)
        {
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
            return Result<Reptile>.Success(newEntity);
        }
        catch (Exception ex)
        {
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
            return Result<Reptile>.Success();
        }
        catch (Exception ex)
        {
            return Result<Reptile>.Failure(ReptileErrors.CantDelete);
        }
    }
}