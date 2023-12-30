using System.Collections.Generic;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public interface ILengthRepository : IGenericRepository<Length>
{
    Task<List<Length>> GetAllForReptile(int reptileId);
}