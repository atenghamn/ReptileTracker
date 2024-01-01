using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.Animal.Model;
using ReptileTracker.EntityFramework;

namespace ReptileTracker.Infrastructure.Persistence;

public class WeightRepository(ReptileContext context) : GenericRepository<Weight>(context), IWeightRepository
{
    public async Task<List<Weight>> GetAllForReptile(int reptileId)
    {
        return await _context.Weights.Where(x => x.ReptileId == reptileId).ToListAsync();
    }
}