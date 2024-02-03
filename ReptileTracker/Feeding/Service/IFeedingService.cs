using System.Collections.Generic;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;
using System.Threading.Tasks;
using System.Threading;

namespace ReptileTracker.Feeding.Service;

public interface IFeedingService
{
    Task<Result<FeedingEvent>> AddFeedingEvent(FeedingEvent feedingEvent, CancellationToken ct);
    Task<Result<FeedingEvent>> GetFeedingEventById(int feedingEventId, CancellationToken ct);
    Task<Result<FeedingEvent>> DeleteFeedingEvent(int feedingEventId, CancellationToken ct);
    Task<Result<FeedingEvent>> UpdateFeedingEvent(FeedingEvent feedingEvent, CancellationToken ct);
    Task<Result<List<FeedingEvent>>> GetFeedingEvents(int reptileId, CancellationToken ct);

}