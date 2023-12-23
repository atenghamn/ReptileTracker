
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReptileTracker.Animal.Model;

[Table("length", Schema = "dbo")]

public class Length
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
    
    [Required]
    [Column("measure")]
    public int Measure { get; set; }
   
    [Required]
    [Column("measurement_date")]
    public DateTime MeasurementDate { get; set; }
}