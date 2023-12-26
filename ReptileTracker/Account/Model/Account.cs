using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Account.Model;

[Table("account", Schema = "dbo")]
public class Account
{
    [Column("id")]
    [Required]
    [Key]
    public int Id { get; set; } 
    
    [Required]
    [StringLength(20)]
    [Column("username")]
    public string Username { get; set; }
    
    [EmailAddress]
    [Required]
    [Column("email")]
    public string Email { get; set; }
    
    [StringLength(60)]
    [Column("name")]
    public string Name { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Column("last_updated")]
    public DateTime LastUpdated { get; set; }
    
    [Column("reset_token")]
    public string ResetToken { get; set; }
    
    [Column("reset_token_expiration")]
    public DateTime ResetTokenExpiration { get; set; }
    
    [NotMapped]
    public ICollection<Reptile> Reptiles { get; set; }
    
}
