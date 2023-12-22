using ReptileTracker.Animal.Model;

namespace ReptileTracker.Shedding.Model;

public class SheddingEvent
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public Reptile Reptile { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}
