using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.Animal.Model;
using ReptileTracker.EntityFramework;

namespace ReptileTracker.Infrastructure.Persistence;

public class LengthRepository(ReptileContext context) : GenericRepository<Length>(context), ILengthRepository
{
    public async Task<List<Length>> GetAllForReptile(int reptileId)
    {
        return await _context.Lengths.Where(x => x.ReptileId == reptileId).ToListAsync();
    }
}