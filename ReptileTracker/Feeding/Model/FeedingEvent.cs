using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Feeding.Model;

[Table("feeding_event", Schema = "dbo")]
public class FeedingEvent
{
    [Column("id")]
    [Required]
    [Key]
    public int Id { get; set; }
   
    [ForeignKey("Reptile")]
    [Column("reptile_id")]
    public int ReptileId { get; set; }
    
    [NotMapped]
    public Reptile Reptile { get; set; }
    
    [Column("date")]
    public DateTime Date { get; set; }
    
    [Column("amount")]
    [Required]
    public int Amount { get; set; }
    
    [Column("food_type")]
    [Required]
    public FoodType FoodType { get; set; }
    
    [Column("notes")]
    [StringLength(60)]
    public string? Notes { get; set; }
}
