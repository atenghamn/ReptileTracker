using System.Threading;
using System.Threading.Tasks;

namespace ReptileTracker.Infrastructure.Persistence;

public interface IAccountRepository : IGenericRepository<Account.Model.Account>
{
    Task<Account.Model.Account> GetByUsername(string username, CancellationToken ct);
    Task<Account.Model.Account> GetByUserId(string id, CancellationToken ct);
}