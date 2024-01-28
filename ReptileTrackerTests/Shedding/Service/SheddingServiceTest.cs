using System.ComponentModel.DataAnnotations;
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
        _mockedSheddingRepository.GetByIdAsync(1, new CancellationToken()).Returns(_sheddingEvent);
    }

    [Test]
    public async Task GetById_ShouldReturnSheddingEvent_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = await _sheddingService.GetSheddingEventById(id, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_sheddingEvent));
        });
    }

    [Test]
    public async Task GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = await _sheddingService.GetSheddingEventById(id, new CancellationToken());
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }
    
    [Test]
    public async Task AddSheddingEvent_ShouldReturnSuccessResult_WhenValidSheddingEventIsProvided()
    {
        var sheddingEvent = new SheddingEvent() { Id = 2, ReptileId = 1, Date = DateTime.Now, Notes = "New shedding event" };
        _mockedSheddingRepository.AddAsync(Arg.Any<SheddingEvent>(), new CancellationToken()).Returns(sheddingEvent);
        var result = await _sheddingService.AddSheddingEvent(sheddingEvent, new CancellationToken());
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(sheddingEvent));
        });
    }
    
    [Test]
    public async Task DeleteSheddingEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = await _sheddingService.DeleteSheddingEvent(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteSheddingEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = await _sheddingService.DeleteSheddingEvent(2, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }

    [Test]
    public async Task UpdateExistingSheddingEvent_ReturnsSuccess()
    {
        var updatedSheddingEvent = new SheddingEvent() { Id = 1, ReptileId = 1, Date = DateTime.Now, Notes = "Updated shedding event" };
        _mockedSheddingRepository.GetByIdAsync(1, new CancellationToken()).Returns(updatedSheddingEvent);
        var result = await _sheddingService.UpdateSheddingEvent(updatedSheddingEvent, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedSheddingEvent));
        });
    }

    [Test]
    public async Task UpdateNonExistingSheddingEvent_ReturnsErrorResponse()
    {
        var nonExistingSheddingEvent = new SheddingEvent() { Id = 2, ReptileId = 1, Date = DateTime.Now, Notes = "Non-existing shedding event" };
        _mockedSheddingRepository.GetById(2).Returns((SheddingEvent)null);
        var result = await _sheddingService.UpdateSheddingEvent(nonExistingSheddingEvent, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.NotFound));
        });
    }

    [Test]
    public async Task GetSheddingEvents_WithValidData_ReturnsSuccess()
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
        var ct = new CancellationToken();
        _mockedSheddingRepository.GetAllForReptile(1, ct).Returns(sheddingEvents);
        var result = await _sheddingService.GetSheddingEvents(1, ct);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(sheddingEvents));
        });
    }

    [Test]
    public async Task GetSheddingEvents_WithInvalidData_ReturnsError()
    {
        var emptySheddingEvents = new List<SheddingEvent>();
        var ct = new CancellationToken();
        _mockedSheddingRepository.GetAllForReptile(2, ct).Returns(emptySheddingEvents);
        var result = await _sheddingService.GetSheddingEvents(1, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(SheddingErrors.EventlistNotFound));
        });
    }
}