using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface ILengthService
{
    Task<Result<Length>> AddLength(Length length, CancellationToken ct);
    Task<Result<Length>> GetLengthById(int lengthId, CancellationToken ct);
    Task<Result<Length>> DeleteLength(int lengthId, CancellationToken ct);
    Task<Result<Length>> UpdateLength(Length length, CancellationToken ct);
    Task<Result<List<Length>>> GetLengths(int reptileId, CancellationToken ct);
}