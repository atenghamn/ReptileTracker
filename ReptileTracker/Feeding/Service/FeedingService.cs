using System;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTracker.Feeding.Service;

public class FeedingService(IGenericRepository<FeedingEvent> feedingRepository) : IFeedingService
{
    public Result AddFeedingEvent(FeedingEvent feedingEvent)
    {
        try
        {
            feedingRepository.Add(feedingEvent);
            feedingRepository.Save();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(FeedingErrors.CantSave);
        }
    }
}