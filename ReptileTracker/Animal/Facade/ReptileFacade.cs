using System;
using System.Collections.Generic;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Shedding.Model;

namespace ReptileTracker.Animal.Facade;

public class ReptileFacade : IReptileFacade
{
    public Result<Reptile> AddNewReptile(int accountId, string reptileName, string species, DateTime birthdate, ReptileType type)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> UpdateReptile(int accountId, Reptile entity)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> DeleteReptile(int accountId, int reptileId)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> GetFullReptileInfo(int accountId, int reptileId)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> AddNewMeasurement(int accountId, int reptileId, Weight? weight = null, Length? length = null)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> UpdateMeasurement(int accountId, int reptileId, Weight? weight = null, Length? length = null)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> DeleteMeasurement(int accountId, int reptileId, int weightId, int lengthId)
    {
        throw new NotImplementedException();
    }

    public Result<SheddingEvent> AddSheddingEvent(int accountId, int reptileId, SheddingEvent sheddingEvent)
    {
        throw new NotImplementedException();
    }

    public Result<SheddingEvent> UpdateSheddingEvent(int accountId, int reptileId, SheddingEvent sheddingEvent)
    {
        throw new NotImplementedException();
    }

    public Result<Reptile> DeleteSheddingEvent(int accountId, int reptileId, int sheddingEventId)
    {
        throw new NotImplementedException();
    }

    public Result<SheddingEvent> GetSheddingEvent(int accountId, int reptileId, int sheddingEventId)
    {
        throw new NotImplementedException();
    }

    public Result<List<SheddingEvent>> GetAllSheddingEventsForReptile(int accountId, int reptileId)
    {
        throw new NotImplementedException();
    }

    public Result<FeedingEvent> AddFeedingEvent(int accountId, int reptileId, FeedingEvent feedingEvent)
    {
        throw new NotImplementedException();
    }

    public Result<FeedingEvent> UpdateFeedingEvent(int accountId, int reptileId, FeedingEvent feedingEvent)
    {
        throw new NotImplementedException();
    }

    public Result<FeedingEvent> DeleteFeedingEvent(int accountId, int reptileId, int feedingEventId)
    {
        throw new NotImplementedException();
    }

    public Result<FeedingEvent> GetFeedingEvent(int accountId, int reptileId, int feedingEventId)
    {
        throw new NotImplementedException();
    }

    public Result<List<FeedingEvent>> GetAllFeedingEventsForReptile(int accountId, int reptileId)
    {
        throw new NotImplementedException();
    }
}