using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IWeightService
{
    Task<Result<Weight>> AddWeight(Weight weight, CancellationToken ct);
    Task<Result<Weight>> GetWeightById(int weightId, CancellationToken ct);
    Task<Result<Weight>> DeleteWeight(int weightId, CancellationToken ct);
    Task<Result<Weight>> UpdateWeight(Weight weight, CancellationToken ct);
    Task<Result<List<Weight>>> GetWeights(int reptileId, CancellationToken ct);
}