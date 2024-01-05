using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Account.Model;

[Table("account", Schema = "dbo")]
public class Account : IdentityUser
{
    [StringLength(60)]
    [Column("FirstName")]
    public string? FirstName { get; set; }
    
    [StringLength(60)]
    [Column("LastName")]
    public string? LastName { get; set; }
    
    [NotMapped]
    public ICollection<Reptile>? Reptiles { get; set; }
    
}
