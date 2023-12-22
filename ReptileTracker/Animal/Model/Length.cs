
namespace ReptileTracker.Animal.Model;

public class Length
{
    public int ReptileId { get; set; }
    public Reptile Reptile { get; set; }
    public int Measure { get; set; }
    public DateTime MeasurementDate { get; set; }
}