using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Errors;

public class ReptileErrors
{
    public static readonly Error CantSave = new Error("Reptile.CantSave", "Cant add this reptile");
    public static readonly Error CantDelete = new Error("Reptile.CantDelete", "Cant delete this reptile event");
    public static readonly Error NotFound = new Error("Reptile.NotFound", "Reptile not found.");
    public static readonly Error CantUpdate = new Error("Reptile.CantUpdate", "Cant update reptile event");
    public static readonly Error NoReptileHistory = new Error("Reptile.NoHistory", "No reptile history found");

    public static readonly Error EventlistNotFound =
        new Error("Reptile.EventlistNotFound", "Cant find the event list");
}