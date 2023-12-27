using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Errors;

public class WeightErrors
{
    public static readonly Error CantSave = new Error("Weight.CantSave", "Cant add this weight");
    public static readonly Error CantDelete = new Error("Weight.CantDelete", "Cant delete this weight event");
    public static readonly Error NotFound = new Error("Weight.NotFound", "Weight not found.");
    public static readonly Error CantUpdate = new Error("Weight.CantUpdate", "Cant update weight event");
    public static readonly Error NoWeightHistory = new Error("Weight.NoHistory", "No weight history found");

    public static readonly Error EventlistNotFound =
        new Error("Weight.EventlistNotFound", "Cant find the event list");
}