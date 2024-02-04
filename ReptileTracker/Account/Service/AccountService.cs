using ReptileTracker.Account.DTO;
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
        public async Task<Result<AccountDTO>> GetAccountById(int accountId, CancellationToken ct)
        {
            var entity = await accountRepository.GetByIdAsync(accountId, ct);
            if (entity == null)
            {
                return Result<AccountDTO>.Failure(AccountErrors.NotFound);
            }

            var dto = AccountDTOMapper.From(entity);
            return Result<AccountDTO>.Success(dto);
        }
    }
}
