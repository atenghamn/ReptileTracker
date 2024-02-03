using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public interface IWeightRepository : IGenericRepository<Weight>
{
    Task<List<Weight>> GetAllForReptile(int reptileId, CancellationToken ct);
}