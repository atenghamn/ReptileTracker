using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.EntityFramework;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Infrastructure.Persistence;

public class SheddingRepository(ReptileContext context) : GenericRepository<SheddingEvent>(context), ISheddingRepository 
{
    public async Task<List<SheddingEvent>> GetAllForReptile(int reptileId)
    {
        return await _context.SheddingEvents.Where((x => x.ReptileId == reptileId)).ToListAsync();
    }
}
