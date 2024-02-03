using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public interface IReptileRepository : IGenericRepository<Reptile>
{
    Task<IEnumerable<Reptile?>> GetByAccount(string accountId, CancellationToken ct);
}