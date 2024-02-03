using ReptileTracker.Account.Errors;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace ReptileTracker.Account.Service
{
    public class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        public async Task<Result<Model.Account>> GetAccountById(int accountId, CancellationToken ct)
        {
            var entity = await accountRepository.GetByIdAsync(accountId, ct);
            return entity == null
                ? Result<Model.Account>.Failure(AccountErrors.NotFound)
                : Result<Model.Account>.Success(entity);
        }
    }
}
