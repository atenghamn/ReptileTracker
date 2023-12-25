using NSubstitute;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
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
        _mockedFeedingRepository.GetById(1).Returns(_feedingEvent);

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

    [Test]
    public void GetFeedingEventById_WithCorrectId_ReturnSuccessResult()
    {
        var result = _feedingService.GetFeedingEventById(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_feedingEvent));
        });
    }

    [Test]
    public void GetFeedingEventById_WithIncorrectId_ReturnsError()
    {
        var result = _feedingService.GetFeedingEventById(100);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.Error, Is.Not.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteFeedingEvent_WithCorretId_ReturnSuccessResult()
    {
        var result = _feedingService.DeleteFeedingEvent(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteFeedingEvent_WithIncorrectId_ReturnsError()
    {
        var result = _feedingService.DeleteFeedingEvent(100);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.Error, Is.EqualTo(FeedingErrors.NotFound));
        });
    }
}