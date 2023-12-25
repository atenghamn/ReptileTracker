using ReptileTracker.Commons;

namespace ReptileTracker.Feeding.Errors;

public static class FeedingErrors
{
    public static readonly Error CantSave = new Error("Feeding.CantSave", "Cant add this feeding");
}