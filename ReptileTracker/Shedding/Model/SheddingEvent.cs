using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReptileTracker.Animal.Model;

namespace ReptileTracker.Shedding.Model;

[Table("shedding_event", Schema = "dbo")]
public class SheddingEvent
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
    
    [Column("notes")]
    public string? Notes { get; set; }
}
