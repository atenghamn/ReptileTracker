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
    private IWeightRepository _mockedWeigthRepository = Substitute.For<IWeightRepository>();
    private Weight _weigth;

    [SetUp]
    public void SetUp()
    {
        _weightService = new WeightService(_mockedWeigthRepository);
        _weigth = new Weight() { ReptileId = 1, Id = 1, Weighing = 10, WeighingDate = DateTime.Now };
        _mockedWeigthRepository.GetByIdAsync(1, new CancellationToken()).Returns(_weigth);
    }
    
    [Test]
    public async Task GetById_ShouldReturnWeigth_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = await _weightService.GetWeightById(id, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_weigth));
        });
    }

    [Test]
    public async Task GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = await _weightService.GetWeightById(id, new CancellationToken());
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }
    
    [Test]
    public async Task AddWeightEvent_ShouldReturnSuccessResult_WhenValidWeightEventIsProvided()
    {
        var ct = new CancellationToken();
        _mockedWeigthRepository.AddAsync(_weigth, ct).Returns(_weigth);
        var result = await _weightService.AddWeight(_weigth, ct);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_weigth));
        });
    }
    
        [Test]
    public async Task DeleteWeigthEvent_WithCorrectId_ReturnsSuccessResult()
    {
        var result = await _weightService.DeleteWeight(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteWeigthEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = await _weightService.DeleteWeight(2, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }

    [Test]
    public async Task UpdateExistingWeigthEvent_ReturnsSuccess()
    {
        var updatedWeight = new Weight() { Id = 1, Weighing = 2, ReptileId = 1, WeighingDate = DateTime.Now };
        var ct = new CancellationToken();
        _mockedWeigthRepository.GetByIdAsync(1, ct).Returns(updatedWeight);
        var result = await _weightService.UpdateWeight(updatedWeight, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedWeight));
        });
    }

    [Test]
    public async Task UpdateNonExistingWeigthEvent_ReturnsErrorResponse()
    {
        var nonExistingWeight = new Weight() { Id = 2, ReptileId = 1, WeighingDate = DateTime.Now };

        var ct = new CancellationToken();

        _mockedWeigthRepository.GetByIdAsync(2, ct).Returns((Weight)null);
        var result = await _weightService.UpdateWeight(nonExistingWeight, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.NotFound));
        });
    }

    [Test]
    public async Task GetWeigthEvents_WithValidData_ReturnsSuccess()
    {
        var weigthEvents = new List<Weight>()
        {
            new Weight() { Id = 1, ReptileId = 1, Weighing = 10, WeighingDate = DateTime.Now.AddDays(-2) },
            new Weight() { Id = 2, ReptileId = 2, Weighing = 11, WeighingDate = DateTime.Now.AddDays(-1) },
            new Weight() { Id = 3, ReptileId = 3, Weighing = 12, WeighingDate = DateTime.Now }
        };

        var ct = new CancellationToken();
        _mockedWeigthRepository.GetAllForReptile(1, ct).Returns(weigthEvents);
        var result = await _weightService.GetWeights(1, ct);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(weigthEvents));
        });
    }

    [Test]
    public async Task GetWeightEvents_WithInvalidData_ReturnsError()
    {
        var emptyWeigthEvents = new List<Weight>();
        var ct = new CancellationToken();

        _mockedWeigthRepository.GetAll().Returns(emptyWeigthEvents);
        var result = await _weightService.GetWeights(2, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(WeightErrors.EventlistNotFound));
        });
    }
}