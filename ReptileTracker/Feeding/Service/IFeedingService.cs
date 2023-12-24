using ReptileTracker.Feeding.Model;

namespace ReptileTracker.Feeding.Service;

public interface IFeedingService
{
    FeedingEvent AddFeedingEvent(FeedingEvent feedingEvent);
}