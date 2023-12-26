using ReptileTracker.Commons;

namespace ReptileTracker.Feeding.Errors;

public static class FeedingErrors
{
    public static readonly Error CantSave = new Error("Feeding.CantSave", "Cant add this feeding");
    public static readonly Error CantDelete = new Error("Feeding.CantDelete", "Cant delete this feeding event");
    public static readonly Error NotFound = new Error("Feeding.NotFound", "Feeding not found.");
    public static readonly Error CantUpdate = new Error("Feeding.CantUpdate", "Cant update feeding event");
    public static readonly Error NoFeedingHistory = new Error("Feeeding.NoHistory", "No feeding history fouynd");

    public static readonly Error EventlistNotFound =
        new Error("Feeding.EventlistNotFound", "Cant find the event list");
}