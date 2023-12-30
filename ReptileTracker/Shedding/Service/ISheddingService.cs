using System.Collections.Generic;
using ReptileTracker.Commons;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Shedding.Service;

public interface ISheddingService
{
    Result<SheddingEvent> AddSheddingEvent(SheddingEvent sheddingEvent);
    Result<SheddingEvent> GetSheddingEventById(int SheddingEventId);
    Result<SheddingEvent> DeleteSheddingEvent(int SheddingEventId);
    Result<SheddingEvent> UpdateSheddingEvent(SheddingEvent SheddingEvent);
    Result<List<SheddingEvent>> GetSheddingEvents(int reptileId);
}