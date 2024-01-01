using System.Collections.Generic;
using System.Threading.Tasks;
using ReptileTracker.Feeding.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public interface IFeedingRepository : IGenericRepository<FeedingEvent>
{
    Task<List<FeedingEvent>> GetAllForReptile(int reptileId);
}