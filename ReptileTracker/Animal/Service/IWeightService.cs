using System.Collections.Generic;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IWeightService
{
    Result<Weight> AddWeight(Weight weight);
    Result<Weight> GetWeightById(int weightId);
    Result<Weight> DeleteWeight(int weightId);
    Result<Weight> UpdateWeight(Weight weight);
    Result<List<Weight>> GetWeights();
}