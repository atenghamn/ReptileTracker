using Microsoft.EntityFrameworkCore;
using ReptileTracker.Db;

namespace ReptileTracker.Infrastructure.Persistence;

public class AccountRepository(ReptileContext context)
    : GenericRepository<Account.Model.Account>(context), IAccountRepository
{
    public async Task<Account.Model.Account?> GetByUsername(string username)
    {
        return await _context.Accounts!.FirstOrDefaultAsync(x => x.Username == username);
    } 
}