using ReptileTracker.Animal.Model;

namespace ReptileTracker.Feeding.Model;

public class FeedingEvent
{
    public int Id { get; set; }
    public int ReptileId { get; set; }
    public Reptile Reptile { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    public FoodType FoodType { get; set; }
    public string? Notes { get; set; }
}
