using System.Collections.Generic;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;

namespace ReptileTracker.Feeding.Service;

public interface IFeedingService
{
    Result<FeedingEvent> AddFeedingEvent(FeedingEvent feedingEvent);
    Result<FeedingEvent> GetFeedingEventById(int feedingEventId);
    Result<FeedingEvent> DeleteFeedingEvent(int feedingEventId);
    Result<FeedingEvent> UpdateFeedingEvent(FeedingEvent feedingEvent);
    Result<List<FeedingEvent>> GetFeedingEvents(int reptileId);

}