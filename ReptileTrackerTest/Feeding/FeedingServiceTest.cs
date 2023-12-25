using NSubstitute;
using NUnit.Framework.Internal;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTrackerTest.Feeding;

[TestFixture]
public class FeedingServiceTest
{

    private IFeedingService _feedingService;
    private IGenericRepository<FeedingEvent> _mockedFeedingRepository = Substitute.For<IGenericRepository<FeedingEvent>>();
    private FeedingEvent _feedingEvent;

    [SetUp]
    public void Setup()
    {
        _feedingEvent = new FeedingEvent()
        {
            Id = 1,
            ReptileId = 1,
            Date = DateTime.Now,
            Amount = 10,
            FoodType = FoodType.CRICKET,
            Notes = "Feeding notes"
        };
        _feedingService = new FeedingService(_mockedFeedingRepository);
        _mockedFeedingRepository.GetById(1).ReturnsForAnyArgs(_feedingEvent);
    }
    
    [Test]
    public void WhenFeedingEventOccurs_WithCorrectValues_ReturnSuccessResult()
    {
        var result = _feedingService.AddFeedingEvent(_feedingEvent);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }
}