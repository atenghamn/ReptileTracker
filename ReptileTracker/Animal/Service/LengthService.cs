using System;
using System.Collections.Generic;
using System.Linq;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTracker.Animal.Service;

public class LengthService(IGenericRepository<Length> lengthRepository) : ILengthService
{
    public Result<Length> AddLength(Length length)
    {
        try
        {
            lengthRepository.Add(length);
            lengthRepository.Save();
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
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
            return Result<Length>.Success();
        }
        catch (Exception ex)
        {
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
            return Result<Length>.Success(length);
        }
        catch (Exception ex)
        {
            return Result<Length>.Failure(LengthErrors.CantUpdate);
        }
    }

    public Result<List<Length>> GetLengths()
    {
        try
        {
            var entities = lengthRepository.GetAll();
            var list = entities.ToList();
            return list.Count < 1 ? Result<List<Length>>.Failure(LengthErrors.NoLengthHistory) : Result<List<Length>>.Success(list);
        }
        catch (Exception ex)
        {
            return Result<List<Length>>.Failure(LengthErrors.EventlistNotFound);
        }
    }
}