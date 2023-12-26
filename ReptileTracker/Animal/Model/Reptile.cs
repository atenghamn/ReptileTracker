using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Animal.Model;

[Table("reptile", Schema = "dbo")]
public class Reptile
{
    [Column("id")]
    [Required]
    [Key]
    public int Id { get; set; }
    
    [Column("name")]
    [StringLength(60)]
    public string Name { get; set; }
    
    [Column("species")]
    [StringLength(60)]
    public string Species { get; set; }
    
    [Column("birthdate")]
    public DateTime Birthdate { get; set; }
    
    [Column("reptile_type")]
    [Required]
    public ReptileType ReptileType { get; set; }
    
    [NotMapped]
    public ICollection<Length> MeasurmentHistory { get; set; }
    
    [NotMapped]
    public ICollection<Weight> WeightHistory { get; set; }
    
    [NotMapped]
    public ICollection<FeedingEvent> FeedingHistory { get; set; }
    
    [NotMapped]
    public ICollection<SheddingEvent> SheddingHistory { get; set; }
    
    [ForeignKey("Account")]
    [Column("account_id")]
    public int AccountId { get; set; }
    
    [NotMapped]
    public Account.Model.Account Account { get; set; }
}
