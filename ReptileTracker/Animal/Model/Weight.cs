namespace ReptileTracker.Animal.Model;

public class Weight
{
    public int ReptileId { get; set; }
    public Reptile Reptile { get; set; }
    public int Weighing { get; set; }
    public DateTime WeighingDate { get; set; }
}