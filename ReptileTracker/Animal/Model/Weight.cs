using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReptileTracker.Animal.Model;

[Table("weight", Schema = "dbo")]
public class Weight
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Reptile")]
    [Column("reptile_id")]
    public int ReptileId { get; set; }
    
    [NotMapped]
    public Reptile Reptile { get; set; }
    
    [Required]
    [Column("weighing")]
    public int Weighing { get; set; }
    
    [Required]
    [Column("date")]
    public DateTime WeighingDate { get; set; }
}