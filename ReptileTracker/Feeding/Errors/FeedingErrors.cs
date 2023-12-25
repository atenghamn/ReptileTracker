using ReptileTracker.Commons;

namespace ReptileTracker.Feeding.Errors;

public static class FeedingErrors
{
    public static readonly Error CantSave = new Error("Feeding.CantSave", "Cant add this feeding");
    public static readonly Error CantDelete = new Error("Feeding.CantDelete", "Cant delete this feeding event");
    public static readonly Error NotFound = new Error("Feeding.NotFound", "Feeding not found.");
}