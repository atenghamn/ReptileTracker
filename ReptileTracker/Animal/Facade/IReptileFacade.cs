using System;
using System.Collections.Generic;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Animal.Facade;

public interface IReptileFacade
{
    Result<Reptile> AddNewReptile(int accountId, string reptileName, string species, DateTime birthdate,
        ReptileType type);
    Result<Reptile> UpdateReptile(int accountId, Reptile entity);
    Result<Reptile> DeleteReptile(int accountId, int reptileId);
    Result<Reptile> GetFullReptileInfo(int accountId, int reptileId);

    Result<Reptile> AddNewMeasurement(int accountId, int reptileId, Weight? weight = null, Length? length = null);
    Result<Reptile> UpdateMeasurement(int accountId, int reptileId, Weight? weight = null, Length? length = null);
    Result<Reptile> DeleteMeasurement(int accountId, int reptileId, int weightId, int lengthId);

    Result<SheddingEvent> AddSheddingEvent(int accountId, int reptileId, SheddingEvent sheddingEvent);
    Result<SheddingEvent> UpdateSheddingEvent(int accountId, int reptileId, SheddingEvent sheddingEvent);
    Result<Reptile> DeleteSheddingEvent(int accountId, int reptileId, int sheddingEventId);
    Result<SheddingEvent> GetSheddingEvent(int accountId, int reptileId, int sheddingEventId);
    Result<List<SheddingEvent>> GetAllSheddingEventsForReptile(int accountId, int reptileId);

    Result<FeedingEvent> AddFeedingEvent(int accountId, int reptileId, FeedingEvent feedingEvent);
    Result<FeedingEvent> UpdateFeedingEvent(int accountId, int reptileId, FeedingEvent feedingEvent);
    Result<FeedingEvent> DeleteFeedingEvent(int accountId, int reptileId, int feedingEventId);
    Result<FeedingEvent> GetFeedingEvent(int accountId, int reptileId, int feedingEventId);
    Result<List<FeedingEvent>> GetAllFeedingEventsForReptile(int accountId, int reptileId);
}