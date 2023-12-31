using System.Collections.Generic;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface ILengthService
{
    Result<Length> AddLength(Length length);
    Result<Length> GetLengthById(int lengthId);
    Result<Length> DeleteLength(int lengthId);
    Result<Length> UpdateLength(Length length);
    Task<Result<List<Length>>> GetLengths(int reptileId);
}