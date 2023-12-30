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
    private IGenericRepository<Length> _mockedLengthRepository = Substitute.For<IGenericRepository<Length>>();
    private Length _length;

    [SetUp]
    public void SetUp()
    {
        _lengthService = new LengthService(_mockedLengthRepository);
        _length = new Length { Id = 1, ReptileId = 1, Measure = 10, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetById(1).Returns(_length);
    }
    
    [Test]
    public void GetById_ShouldReturnLength_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = _lengthService.GetLengthById(id);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_length));
        });
    }

    [Test]
    public void GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = _lengthService.GetLengthById(id);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }
    
    [Test]
    public void AddSheddingEvent_ShouldReturnSuccessResult_WhenValidSheddingEventIsProvided()
    {
        _mockedLengthRepository.Add(_length).Returns(_length);
        var result = _lengthService.AddLength(_length);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_length));
        });
    }
    
        [Test]
    public void DeleteLengthEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = _lengthService.DeleteLength(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteLengthEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = _lengthService.DeleteLength(2);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }

    [Test]
    public void UpdateExistingLengthEvent_ReturnsSuccess()
    {
        var updatedLength = new Length() { Id = 1, Measure = 2, ReptileId = 1, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetById(1).Returns(updatedLength);
        var result = _lengthService.UpdateLength(updatedLength);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedLength));
        });
    }

    [Test]
    public void UpdateNonExistingLengthEvent_ReturnsErrorResponse()
    {
        var nonExistingLength = new Length() { Id = 2, ReptileId = 1, MeasurementDate = DateTime.Now };
        _mockedLengthRepository.GetById(2).Returns((Length)null);
        var result = _lengthService.UpdateLength(nonExistingLength);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NotFound));
        });
    }

    [Test]
    public void GetSheddingEvents_WithValidData_ReturnsSuccess()
    {
        var lengthEvents = new[]
        {
            new Length { Id = 1, ReptileId = 1, Measure = 10, MeasurementDate = DateTime.Now },
            new Length { Id = 2, ReptileId = 1, Measure = 20, MeasurementDate = DateTime.Now },
            new Length { Id = 3, ReptileId = 1, Measure = 30, MeasurementDate = DateTime.Now }
        };
        _mockedLengthRepository.GetAll().Returns(lengthEvents);
        var result = _lengthService.GetLengths();
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(lengthEvents));
        });
    }

    [Test]
    public void GetSheddingEvents_WithInvalidData_ReturnsError()
    {
        var emptyLengthEvents = new List<Length>();
        _mockedLengthRepository.GetAll().Returns(emptyLengthEvents);
        var result = _lengthService.GetLengths();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(LengthErrors.NoLengthHistory));
        });
    }
}