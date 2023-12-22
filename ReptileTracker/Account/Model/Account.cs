using ReptileTracker.Animal.Model;

namespace ReptileTracker.Account.Model;

public class Account
{
    public int AccountId;
    public string Username { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string ResetToken { get; set; }
    public DateTime ResetTokenExpiration { get; set; }
    public List<Reptile> Reptiles { get; set; }
    
}
