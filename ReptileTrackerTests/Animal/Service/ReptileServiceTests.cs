using NSubstitute;
using ReptileTracker.Account.Model;
using ReptileTracker.Animal.Errors;
using ReptileTracker.Animal.Model;
using ReptileTracker.Animal.Service;
using ReptileTracker.Commons;
using ReptileTracker.Feeding.Model;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Shedding.Model;

namespace ReptileTrackerTests.Animal.Service;

[TestFixture]
public class ReptileServiceTests
{
    private IReptileService _reptileService;
    private IGenericRepository<Reptile> _moockedReptileRepository = Substitute.For<IGenericRepository<Reptile>>();
    private Reptile _reptile;

    [SetUp]
    public void SetUo()
    {
        Account testAccount = new Account()
        {
            Username = "testUsername",
            Email = "testEmail@test.com",
            Name = "testName",
            Password = "testPassword",
            Created = DateTime.Now,
            LastUpdated = DateTime.Now,
            ResetToken = "testResetToken",
            ResetTokenExpiration = DateTime.Now
        };
        
        _reptile = new Reptile()
        {
            Id = 1,
            ReptileType = ReptileType.LIZARD,
            Account = testAccount,
            Name = "Godzilla",
            Birthdate = new DateTime(2023, 11, 25),
            Species = "Crested gecko",
            MeasurmentHistory = new List<Length>(),
            WeightHistory = new List<Weight>(),
            FeedingHistory = new List<FeedingEvent>(),
            SheddingHistory = new List<SheddingEvent>(),
        };
        
        _moockedReptileRepository.GetById(1).Returns(_reptile);
        _reptileService = new ReptileService(_moockedReptileRepository);
    }
    
      [Test]
    public void GetById_ShouldReturnReptile_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = _reptileService.GetReptileById(id);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_reptile));
        });
    }

    [Test]
    public void GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = _reptileService.GetReptileById(id);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }
    
    [Test]
    public void AddReptile_ShouldReturnSuccessResult_WhenValidReptileDataIsProvided()
    {
        _moockedReptileRepository.Add(_reptile).Returns(_reptile);
        var result = _reptileService.CreateReptile(
            name: "Red", 
            species: "Blood Python", 
            birthdate: DateTime.Now, 
            type: ReptileType.SNAKE);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data.Name, Is.EqualTo("Red"));
            Assert.That(result.Data.Species, Is.EqualTo("Blood Python"));
        });
    }
    
        [Test]
    public void DeleteReptile_WithCorrectId_ReturnsSuccessResult()
    {
        var result = _reptileService.DeleteReptile(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public void DeleteReptileEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = _reptileService.DeleteReptile(2);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }

    [Test]
    public void UpdateExistingReptile_ReturnsSuccess()
    {
        var updatedReptile = new Reptile()
        {
            Id = 1,
            ReptileType = ReptileType.CROCODILIAN,
            AccountId = 1,
            Name = "Godzilla",
            Birthdate = new DateTime(2023, 11, 25),
            Species = "Alligtor",
            MeasurmentHistory = new List<Length>(),
            WeightHistory = new List<Weight>(),
            FeedingHistory = new List<FeedingEvent>(),
            SheddingHistory = new List<SheddingEvent>(),
        };
_moockedReptileRepository.GetById(1).Returns(updatedReptile);
        var result = _reptileService.UpdateReptile(updatedReptile);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedReptile));
        });
    }

    [Test]
    public void UpdateNonExistingReptile_ReturnsErrorResponse()
    {
        var nonExistingReptile = new Reptile()
        {
            Id = 2,
            ReptileType = ReptileType.CROCODILIAN,
            AccountId = 1,
            Name = "Godzilla",
            Birthdate = new DateTime(2023, 11, 25),
            Species = "Alligtor",
            MeasurmentHistory = new List<Length>(),
            WeightHistory = new List<Weight>(),
            FeedingHistory = new List<FeedingEvent>(),
            SheddingHistory = new List<SheddingEvent>(),
        };
        _moockedReptileRepository.GetById(2).Returns((Reptile)null);
        var result = _reptileService.UpdateReptile(nonExistingReptile);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }

}