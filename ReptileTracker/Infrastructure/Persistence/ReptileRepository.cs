using Microsoft.EntityFrameworkCore;
using ReptileTracker.Animal.Model;
using ReptileTracker.Db;

namespace ReptileTracker.Infrastructure.Persistence;

public class ReptileRepository(ReptileContext context) : GenericRepository<Reptile>(context), IReptileRepository
{
    public async Task<IEnumerable<Reptile?>> GetByAccount(int accountId)
    {
        var reptiles = await _context.Reptiles.Where(x => x.AccountId == accountId).ToListAsync();
        return reptiles;
    }
}