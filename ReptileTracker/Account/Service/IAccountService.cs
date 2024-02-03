using ReptileTracker.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace ReptileTracker.Account.Service
{
    public interface IAccountService
    {
        Task<Result<Model.Account>> GetAccountById(int accountId, CancellationToken ct);
    }

}
