namespace ReptileTracker.Account.DTO
{
    public class AccountDTO
    {
        public AccountDTO()
        {
        }

        public AccountDTO(string id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;


       
    }
}
