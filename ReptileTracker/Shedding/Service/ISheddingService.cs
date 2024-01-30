using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Commons;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Shedding.Service;

public interface ISheddingService
{
    Task<Result<SheddingEvent>> AddSheddingEvent(SheddingEvent sheddingEvent, CancellationToken ct);
    Task<Result<SheddingEvent>> GetSheddingEventById(int SheddingEventId, CancellationToken ct);
    Task<Result<SheddingEvent>> DeleteSheddingEvent(int SheddingEventId, CancellationToken ct);
    Task<Result<SheddingEvent>> UpdateSheddingEvent(SheddingEvent SheddingEvent, CancellationToken ct);
    Task<Result<List<SheddingEvent>>> GetSheddingEvents(int reptileId, CancellationToken ct);
}