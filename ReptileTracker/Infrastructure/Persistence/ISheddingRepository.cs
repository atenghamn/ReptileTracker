using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public interface ISheddingRepository : IGenericRepository<SheddingEvent>
{
    Task<List<SheddingEvent>> GetAllForReptile(int reptileId, CancellationToken ct);
}