using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.Animal.Model;
using ReptileTracker.EntityFramework;

namespace ReptileTracker.Infrastructure.Persistence;
public class ReptileRepository(ReptileContext context) : GenericRepository<Reptile>(context), IReptileRepository
{
    public async Task<IEnumerable<Reptile?>> GetByAccount(int accountId)
    {
        var reptiles = await _context.Reptiles.Where(x => x.AccountId == accountId).ToListAsync();
        return reptiles;
    }
}