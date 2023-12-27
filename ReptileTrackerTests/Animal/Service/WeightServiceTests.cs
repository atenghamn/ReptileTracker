using NSubstitute;
using ReptileTracker.Commons;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.Animal.Errors;

namespace ReptileTrackerTests.Animal.Service;

[TestFixture]
public class WeightServiceTests
{
    private IWeightService _weightService;
    private IGenericRepository<Weight> _mockedWeigthRepository = Substitute.For<IGenericRepository<Weight>>();
    private Weight _weigth;

    [SetUp]
    public void SetUp()
    {
        _weightService = new WeightService(_mockedWeigthRepository);
        _weigth = new Weight() { ReptileId = 1, Id = 1, Weighing = 10, WeighingDate = DateTime.Now };
        _mockedWeigthRepository.GetById(1).Returns(_weigth);
    }
    
    [Test]
    public void GetById_ShouldReturnWeigth_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = _weightService.GetWeightById(id);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_weigth));
        });
    }

    [Test]
    public void GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = _weightService.GetWeightById(id);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }
    
    [Test]
    public void AddWeightEvent_ShouldReturnSuccessResult_WhenValidWeightEventIsProvided()
    {
        _mockedWeigthRepository.Add(_weigth).Returns(_weigth);
        var result = _weightService.AddWeight(_weigth);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_weigth));
        });
    }
    
        [Test]
    public void DeleteWeigthEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = _weightService.DeleteWeight(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteWeigthEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = _weightService.DeleteWeight(2);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }

    [Test]
    public void UpdateExistingWeigthEvent_ReturnsSuccess()
    {
        var updatedWeight = new Weight() { Id = 1, Weighing = 2, ReptileId = 1, WeighingDate = DateTime.Now };
        _mockedWeigthRepository.GetById(1).Returns(updatedWeight);
        var result = _weightService.UpdateWeight(updatedWeight);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedWeight));
        });
    }

    [Test]
    public void UpdateNonExistingWeigthEvent_ReturnsErrorResponse()
    {
        var nonExistingWeight = new Weight() { Id = 2, ReptileId = 1, WeighingDate = DateTime.Now };
        _mockedWeigthRepository.GetById(2).Returns((Weight)null);
        var result = _weightService.UpdateWeight(nonExistingWeight);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }

    [Test]
    public void GetWeigthEvents_WithValidData_ReturnsSuccess()
    {
        var weigthEvents = new[]
        {
            new Weight() { Id = 1, ReptileId = 1, Weighing = 10, WeighingDate = DateTime.Now.AddDays(-2) },
            new Weight() { Id = 2, ReptileId = 2, Weighing = 11, WeighingDate = DateTime.Now.AddDays(-1) },
            new Weight() { Id = 3, ReptileId = 3, Weighing = 12, WeighingDate = DateTime.Now }
        };
        _mockedWeigthRepository.GetAll().Returns(weigthEvents);
        var result = _weightService.GetWeights();
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(weigthEvents));
        });
    }

    [Test]
    public void GetWeightEvents_WithInvalidData_ReturnsError()
    {
        var emptyWeigthEvents = new List<Weight>();
        _mockedWeigthRepository.GetAll().Returns(emptyWeigthEvents);
        var result = _weightService.GetWeights();
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NoWeightHistory));
        });
    }
}