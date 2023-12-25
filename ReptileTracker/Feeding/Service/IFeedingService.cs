using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;

namespace ReptileTracker.Feeding.Service;

public interface IFeedingService
{
    Result AddFeedingEvent(FeedingEvent feedingEvent);
}