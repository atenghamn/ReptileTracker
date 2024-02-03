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
    private IReptileRepository _moockedReptileRepository = Substitute.For<IReptileRepository>();
    private IAccountRepository _mockedAccountRepository = Substitute.For<IAccountRepository>();
    private Reptile _reptile;

    [SetUp]
    public void SetUp()
    {
        Account testAccount = new Account()
        {
            UserName = "testUsername",
            Email = "testEmail@test.com",
            FirstName = "Jane",
            LastName = "Doe"
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

        _mockedAccountRepository.GetByUsername("kalle@test.se", new CancellationToken()).Returns(testAccount);
        _moockedReptileRepository.GetByIdAsync(1, new CancellationToken()).Returns(_reptile);
        _reptileService = new ReptileService(_moockedReptileRepository, _mockedAccountRepository);
    }

    [Test]
    public async Task GetById_ShouldReturnReptile_WhenValidIdIsProvided()
    {
        const int id = 1;
        var result = await _reptileService.GetReptileById(id, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(_reptile));
        });
    }

    [Test]
    public async Task GetById_ShouldReturnErrorResult_WhenInvalidIdIsProvided()
    {
        const int id = 2;
        var result = await _reptileService.GetReptileById(id, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }

    [Test]
    public async Task AddReptile_ShouldReturnSuccessResult_WhenValidReptileDataIsProvided()
    {
        var ct = new CancellationToken();
        _moockedReptileRepository.AddAsync(_reptile, ct).Returns(_reptile);
        var result = await _reptileService.CreateReptile(
            name: "Red",
            species: "Blood Python",
            birthdate: DateTime.Now,
            type: ReptileType.SNAKE,
            username: "kalle@test.se",
            ct: ct);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data.Name, Is.EqualTo("Red"));
            Assert.That(result.Data.Species, Is.EqualTo("Blood Python"));
        });
    }

    [Test]
    public async Task DeleteReptile_WithCorrectId_ReturnsSuccessResult()
    {
        var result = await _reptileService.DeleteReptile(1, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
        });
    }

    [Test]
    public async Task DeleteReptileEvent_WithIncorrectId_ReturnsErrorResult()
    {
        var result = await _reptileService.DeleteReptile(2, new CancellationToken());
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }

    [Test]
    public async Task UpdateExistingReptile_ReturnsSuccess()
    {
        var updatedReptile = new Reptile()
        {
            Id = 1,
            ReptileType = ReptileType.CROCODILIAN,
            AccountId = "1",
            Name = "Godzilla",
            Birthdate = new DateTime(2023, 11, 25),
            Species = "Alligtor",
            MeasurmentHistory = new List<Length>(),
            WeightHistory = new List<Weight>(),
            FeedingHistory = new List<FeedingEvent>(),
            SheddingHistory = new List<SheddingEvent>(),
        };

        var ct = new CancellationToken();

        _moockedReptileRepository.GetByIdAsync(1, ct).Returns(updatedReptile);
        var result = await _reptileService.UpdateReptile(updatedReptile, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(updatedReptile));
        });
    }

    [Test]
    public async Task UpdateNonExistingReptile_ReturnsErrorResponse()
    {
        var nonExistingReptile = new Reptile()
        {
            Id = 2,
            ReptileType = ReptileType.CROCODILIAN,
            AccountId = "1",
            Name = "Godzilla",
            Birthdate = new DateTime(2023, 11, 25),
            Species = "Alligtor",
            MeasurmentHistory = new List<Length>(),
            WeightHistory = new List<Weight>(),
            FeedingHistory = new List<FeedingEvent>(),
            SheddingHistory = new List<SheddingEvent>(),
        };

        var ct = new CancellationToken();

        _moockedReptileRepository.GetByIdAsync(2, ct).Returns((Reptile)null);
        var result = await _reptileService.UpdateReptile(nonExistingReptile, ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }

    [Test]
    public async Task GetReptilesByAccount_GivenValidId_ReturnListOfReptiles()
    {
        var reptiles = new List<Reptile>()
        {
            new Reptile()
            {
                AccountId = "1",
                ReptileType = ReptileType.CROCODILIAN,
                Name = "Godzilla",
                Birthdate = new DateTime(2023, 11, 25),
                Species = "Alligtor",
                MeasurmentHistory = new List<Length>(),
                WeightHistory = new List<Weight>(),
                FeedingHistory = new List<FeedingEvent>(),
                SheddingHistory = new List<SheddingEvent>(),
            },
            new Reptile()
            {
                AccountId = "1",
                ReptileType = ReptileType.CROCODILIAN,
                Name = "Bridezilla",
                Birthdate = new DateTime(2023, 11, 25),
                Species = "Alligtor",
                MeasurmentHistory = new List<Length>(),
                WeightHistory = new List<Weight>(),
                FeedingHistory = new List<FeedingEvent>(),
                SheddingHistory = new List<SheddingEvent>(),
            }
        };

        var ct = new CancellationToken();

        _moockedReptileRepository.GetByAccount("12345", ct).Returns(reptiles);
        _mockedAccountRepository.GetByUsername("janedoe@example.com", ct).Returns(new Account() { Id = "12345" });

        var result = await _reptileService.GetReptilesByAccount("janedoe@example.com", ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(Error.None));
            Assert.That(result.Data, Is.EqualTo(reptiles));
        });
    }

    [Test]
    public async Task GetReptilesByAccount_WithInvalidId_ReturnsError()
    {
        var ct = new CancellationToken();
        _moockedReptileRepository.GetByAccount("2", ct).Returns((Task<IEnumerable<Reptile?>>)null);
        var result = await _reptileService.GetReptilesByAccount("2", ct);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.EqualTo(true));
            Assert.That(result.Error, Is.EqualTo(ReptileErrors.NotFound));
        });
    }
}