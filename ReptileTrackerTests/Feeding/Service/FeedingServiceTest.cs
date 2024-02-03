using NSubstitute;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Errors;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Feeding.Service;
using ReptileTracker.Infrastructure.Persistence;
using System.Runtime.CompilerServices;

namespace ReptileTrackerTests.Feeding.Service;

[TestFixture]
public class FeedingServiceTest
{

    private IFeedingService _feedingService;
    private IFeedingRepository _mockedFeedingRepository = Substitute.For<IFeedingRepository>();
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
        _mockedFeedingRepository.GetByIdAsync(1, new CancellationToken()).Returns(_feedingEvent);

    }

    [Test]
    public async Task WhenFeedingEventOccurs_WithCorrectValues_ReturnSuccessResult()
    {
        var result = await _feedingService.AddFeedingEvent(_feedingEvent, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task GetFeedingEventById_WithCorrectId_ReturnSuccessResult()
    {
        var result = await _feedingService.GetFeedingEventById(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_feedingEvent));
        });
    }

    [Test]
    public async Task GetFeedingEventById_WithIncorrectId_ReturnsError()
    {
        var result = await _feedingService.GetFeedingEventById(100, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.Error, Is.Not.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteFeedingEvent_WithCorretId_ReturnSuccessResult()
    {
        var result = await _feedingService.DeleteFeedingEvent(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteFeedingEvent_WithIncorrectId_ReturnsError()
    {
        var result = await _feedingService.DeleteFeedingEvent(100, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.Error, Is.EqualTo(FeedingErrors.NotFound));
        });
    }

    [Test]
    public async Task UpdateExistingFeedingEvent_ReturnsSuccess()
    {
        var result = await _feedingService.UpdateFeedingEvent(_feedingEvent, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Data, Is.EqualTo(_feedingEvent));
        });
    }

    [Test]
    public async Task UpdateNonExistingFeedingEvent_ReturnsError()
    {
        var result = await _feedingService.UpdateFeedingEvent(null, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.Error, Is.EqualTo(FeedingErrors.CantUpdate));
        });
    }

    [Test]
    public async Task GetFeedingEvents_WithValidData_ReturnsSuccess()
    {
        var feedingEvents = new List<FeedingEvent>()
        {
            new FeedingEvent()
            {
                Id = 1,
                ReptileId = 1,
                Date = DateTime.Now,
                Amount = 10,
                FoodType = FoodType.CRICKET,
                Notes = "Feeding notes"
            },
            new FeedingEvent()
            {
                Id = 2,
                ReptileId = 2,
                Date = DateTime.Now.AddDays(-1),
                Amount = 5,
                FoodType = FoodType.MEALWORM,
                Notes = "Another feeding event"
            }
        };
        _mockedFeedingRepository.GetAllForReptile(1).Returns(feedingEvents);

        var result = await _feedingService.GetFeedingEvents(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(feedingEvents));
        });
    }

    [Test]
    public async Task GetFeedingEvents_WithInvalidData_ReturnsError()
    {
        var result = await _feedingService.GetFeedingEvents(2, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(FeedingErrors.EventlistNotFound));
        });
    }
}