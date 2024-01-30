using NSubstitute;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;

namespace ReptileTrackerTests.Animal.Service;

[TestFixture]
public class LengthServiceTests
{
    private ILengthService _lengthService;
    private ILengthRepository _mockedLengthRepository = Substitute.For<ILengthRepository>();
    private Length _length;

    [SetUp]
    public void SetUp()
    {
        _lengthService = new LengthService(_mockedLengthRepository);
        _length = new Length { Id = 1, ReptileId = 1, Measure = 10, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetByIdAsync(1, new CancellationToken()).Returns(_length);
    }
    
    [Test]
    public async Task GetById_ShouldReturnLength_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = await _lengthService.GetLengthById(id, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_length));
        });
    }

    [Test]
    public async Task GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = await _lengthService.GetLengthById(id, new CancellationToken());
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }
    
    [Test]
    public async Task AddSheddingEvent_ShouldReturnSuccessResult_WhenValidSheddingEventIsProvided()
    {
        var ct = new CancellationToken();
        _mockedLengthRepository.AddAsync(_length, ct).Returns(_length);
        var result = await _lengthService.AddLength(_length, ct);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_length));
        });
    }
    
        [Test]
    public async Task DeleteLengthEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = await _lengthService.DeleteLength(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteLengthEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = await _lengthService.DeleteLength(2, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }

    [Test]
    public async Task UpdateExistingLengthEvent_ReturnsSuccess()
    {
        var ct = new CancellationToken();
        var updatedLength = new Length() { Id = 1, Measure = 2, ReptileId = 1, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetByIdAsync(1, ct).Returns(updatedLength);
        var result = await _lengthService.UpdateLength(updatedLength, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedLength));
        });
    }

    [Test]
    public async Task UpdateNonExistingLengthEvent_ReturnsErrorResponse()
    {
        var ct = new CancellationToken();
        var nonExistingLength = new Length() { Id = 2, ReptileId = 1, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetByIdAsync(2, ct).Returns((Length)null);
        var result = await _lengthService.UpdateLength(nonExistingLength, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }

    [Test]
    public async Task GetSheddingEvents_WithValidData_ReturnsSuccess()
    {
        var ct = new CancellationToken();
        var lengthEvents = new List<Length>()
        {
            new Length { Id = 1, ReptileId = 1, Measure = 10, MeasurementDate = DateTime.Now },
            new Length { Id = 2, ReptileId = 1, Measure = 20, MeasurementDate = DateTime.Now },
            new Length { Id = 3, ReptileId = 1, Measure = 30, MeasurementDate = DateTime.Now }
        };
        _mockedLengthRepository.GetAllForReptile(1).Returns(lengthEvents);
        var result = await _lengthService.GetLengths(1, ct);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(lengthEvents));
        });
    }

    [Test]
    public async Task GetSheddingEvents_WithInvalidData_ReturnsError()
    {
        var ct = new CancellationToken();
        var emptyLengthEvents = new List<Length>();
        _mockedLengthRepository.GetAllAsync(ct).Returns(emptyLengthEvents);
        var result = await _lengthService.GetLengths(2, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.EventlistNotFound));
        });
    }
}