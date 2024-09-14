using ReptileTracker.Commons;

namespace ReptileTracker.Account.Errors
{
    public static class AccountErrors
    {
        public static readonly Error CantSave = new("Account.CantSave", "Cant add this account information");
        public static readonly Error CantDelete = new("Account.CantDelete", "Cant delete this account");
        public static readonly Error NotFound = new("Account.NotFound", "Account not found.");
        public static readonly Error CantUpdate = new("Account.CantUpdate", "Cant update account information");
    }
}