namespace ReptileTracker.Account.DTO
{
    public interface AccountDTOMapper
    {
        static AccountDTO From (Model.Account entity)
        {
            if (entity == null) { return null; }

            return new AccountDTO(
                id: entity.Id,
                firstName: entity.FirstName,
                lastName: entity.LastName,
                email: entity.UserName);
        }
    }
}
