using ReptileTracker.Account.DTO;
using ReptileTracker.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace ReptileTracker.Account.Service
{
    public interface IAccountService
    {
        Task<Result<AccountDTO>> GetAccountById(int accountId, CancellationToken ct);
    }

}
