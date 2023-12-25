using System;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTracker.Feeding.Service;

public sealed class FeedingService(IGenericRepository<FeedingEvent> feedingRepository) : IFeedingService
{
    public Result<FeedingEvent> GetFeedingEventById(int feedingId)
    {
            var entity = feedingRepository.GetById(feedingId);
            return entity == null
                ? Result<FeedingEvent>.Failure(FeedingErrors.NotFound)
                : Result<FeedingEvent>.Success(entity);
            
    }
    
    public Result<FeedingEvent> AddFeedingEvent(FeedingEvent feedingEvent)
    {
        try
        {
            feedingRepository.Add(feedingEvent);
            feedingRepository.Save();
            return Result<FeedingEvent>.Success(feedingEvent);
        }
        catch (Exception ex)
        {
            return Result<FeedingEvent>.Failure(FeedingErrors.CantSave);
        }
    }

    public Result<FeedingEvent> DeleteFeedingEvent(int id)
    {
        try
        {
            var entity = GetFeedingEventById(id);
            if (entity.Data == null) return Result<FeedingEvent>.Failure(FeedingErrors.NotFound);
            feedingRepository.Delete(entity.Data);
            feedingRepository.Save();
            return Result<FeedingEvent>.Success();
        }
        catch (Exception ex)
        {
            return Result<FeedingEvent>.Failure(FeedingErrors.CantDelete);
        }
    }
}