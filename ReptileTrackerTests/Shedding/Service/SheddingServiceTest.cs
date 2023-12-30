using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using NuGet.Frameworks;
using ReptileTracker.Commons;
using ReptileTracker.EntityFramework;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Shedding.Errors;
using ReptileTracker.Shedding.Model;
using ReptileTracker.Shedding.Service;

namespace ReptileTrackerTests.Shedding.Service;

[TestFixture]
public class SheddingServiceTest
{
    private ISheddingService _sheddingService;
    private ISheddingRepository _mockedSheddingRepository =
        Substitute.For<ISheddingRepository>();

    private SheddingEvent _sheddingEvent;

    [SetUp]
    public void SetUp()
    {
        _sheddingEvent = new SheddingEvent()
        {
            Id = 1,
            ReptileId = 1,
            Date = DateTime.Now,
            Notes = "All went well!"
        };
        _sheddingService = new SheddingService(_mockedSheddingRepository);
        _mockedSheddingRepository.GetById(1).Returns(_sheddingEvent);
    }

    [Test]
    public void GetById_ShouldReturnSheddingEvent_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = _sheddingService.GetSheddingEventById(id);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_sheddingEvent));
        });
    }

    [Test]
    public void GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = _sheddingService.GetSheddingEventById(id);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }
    
    [Test]
    public void AddSheddingEvent_ShouldReturnSuccessResult_WhenValidSheddingEventIsProvided()
    {
        var sheddingEvent = new SheddingEvent() { Id = 2, ReptileId = 1, Date = DateTime.Now, Notes = "New shedding event" };
        _mockedSheddingRepository.AddAsync(Arg.Any<SheddingEvent>()).Returns(sheddingEvent);
        var result = _sheddingService.AddSheddingEvent(sheddingEvent);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(sheddingEvent));
        });
    }
    
    [Test]
    public void DeleteSheddingEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = _sheddingService.DeleteSheddingEvent(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteSheddingEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = _sheddingService.DeleteSheddingEvent(2);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }

    [Test]
    public void UpdateExistingSheddingEvent_ReturnsSuccess()
    {
        var updatedSheddingEvent = new SheddingEvent() { Id = 1, ReptileId = 1, Date = DateTime.Now, Notes = "Updated shedding event" };
        _mockedSheddingRepository.GetById(1).Returns(updatedSheddingEvent);
        var result = _sheddingService.UpdateSheddingEvent(updatedSheddingEvent);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedSheddingEvent));
        });
    }

    [Test]
    public void UpdateNonExistingSheddingEvent_ReturnsErrorResponse()
    {
        var nonExistingSheddingEvent = new SheddingEvent() { Id = 2, ReptileId = 1, Date = DateTime.Now, Notes = "Non-existing shedding event" };
        _mockedSheddingRepository.GetById(2).Returns((SheddingEvent)null);
        var result = _sheddingService.UpdateSheddingEvent(nonExistingSheddingEvent);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }

    [Test]
    public void GetSheddingEvents_WithValidData_ReturnsSuccess()
    {
        var sheddingEvents = new List<SheddingEvent>() { 
            new SheddingEvent()
        {
            Id = 1, ReptileId = 1, 
            Date = DateTime.Now, 
            Notes = "Shedding event 1"
        }, 
            new SheddingEvent()
            {
                Id = 2, ReptileId = 1, 
                Date = DateTime.Now, 
                Notes = "Shedding event 2"
            }, 
            new SheddingEvent()
            {
                Id = 3, 
                ReptileId = 2, 
                Date = DateTime.Now, 
                Notes = "Shedding event 3"
            } };
        _mockedSheddingRepository.GetAllForReptile(1).Returns(sheddingEvents);
        var result = _sheddingService.GetSheddingEvents(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(sheddingEvents));
        });
    }

    [Test]
    public void GetSheddingEvents_WithInvalidData_ReturnsError()
    {
        var emptySheddingEvents = new List<SheddingEvent>();
        _mockedSheddingRepository.GetAllForReptile(2).Returns(emptySheddingEvents);
        var result = _sheddingService.GetSheddingEvents(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.EventlistNotFound));
        });
    }
}