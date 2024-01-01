using System.Collections.Generic;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IWeightService
{
    Result<Weight> AddWeight(Weight weight);
    Result<Weight> GetWeightById(int weightId);
    Result<Weight> DeleteWeight(int weightId);
    Result<Weight> UpdateWeight(Weight weight);
    Task<Result<List<Weight>>> GetWeights(int reptileId);
}