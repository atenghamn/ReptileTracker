using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.EntityFramework;

namespace ReptileTracker.Infrastructure.Persistence;

public class AccountRepository(ReptileContext context)
    : GenericRepository<Account.Model.Account>(context), IAccountRepository
{
    public async Task<Account.Model.Account?> GetByUsername(string username, CancellationToken ct) => 
        await _context.Accounts!.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken: ct);

    public async Task<Account.Model.Account?> GetByUserId(string accountId, CancellationToken ct) =>
        await _context.Accounts!.FirstOrDefaultAsync(x => x.Id == accountId, ct);
}