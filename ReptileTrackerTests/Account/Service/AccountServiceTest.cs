using NSubstitute;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Account.Service;
using ReptileTracker.Commons;
using ReptileTracker.Account.Errors;

namespace ReptileTrackerTests.Account.Service
{
    [TestFixture]
    public class AccountServiceTest
    {

        IAccountService _accountService;
        IAccountRepository _mockedAccountRepository = Substitute.For<IAccountRepository>();


        [SetUp]
        public void SetUp()
        {
            _accountService = new AccountService(_mockedAccountRepository);
        }

        [Test]
        public async Task WhenACorrectAccountIdIsGiven_ReturnAccount()
        {
            var account = new ReptileTracker.Account.Model.Account()
            {
                Id = "1",
                FirstName = "Jane",
                LastName = "Doe",
                UserName= "jane@doe.com"
            };

            var ct = new CancellationToken();

            _mockedAccountRepository.GetByIdAsync(1, ct).Returns(account);

            var result = await _accountService.GetAccountById(1, ct);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data?.Id, Is.EqualTo(account.Id));
                Assert.That(result.Data?.FirstName, Is.EqualTo(account.FirstName));
                Assert.That(result.Data?.LastName, Is.EqualTo(account.LastName));
                Assert.That(result?.Data.Email, Is.EqualTo(account.UserName));
            });

        }

        [Test]
        public async Task WhenFirstNameNotPresent_ReturnAccount()
        {
            var account = new ReptileTracker.Account.Model.Account()
            {
                Id = "2",
                LastName = "Doe"
            };

            var ct = new CancellationToken();

            _mockedAccountRepository.GetByIdAsync(2, ct).Returns(account);

            var result = await _accountService.GetAccountById(2, ct);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data?.Id, Is.EqualTo("2"));
                Assert.That(result.Data?.FirstName, Is.EqualTo(null));
                Assert.That(result.Data?.LastName, Is.EqualTo("Doe"));
            });

        }

        [Test]
        public async Task WhenLastNameNotPresent_ReturnAccount()
        {
            var account = new ReptileTracker.Account.Model.Account()
            {
                Id = "3",
                LastName = "Doe"
            };

            var ct = new CancellationToken();

            _mockedAccountRepository.GetByIdAsync(3, ct).Returns(account);

            var result = await _accountService.GetAccountById(3, ct);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data?.Id, Is.EqualTo("3"));
                Assert.That(result.Data?.LastName, Is.EqualTo("Doe"));
            });

        }

        [Test]
        public async Task WhenAnAccountOnlyHasIdAndUsername_ReturnAccount()
        {
            var account = new ReptileTracker.Account.Model.Account()
            {
                Id = "4",
                UserName = "jane@doe.com"
            };
            var ct = new CancellationToken();
            _mockedAccountRepository.GetByIdAsync(4, ct).Returns(account);

            var result = await _accountService.GetAccountById(4, ct);

            Assert.Multiple(() =>
            {
                Assert.That(result.Data?.Id, Is.EqualTo(account.Id));
                Assert.That(result?.Data.Email, Is.EqualTo(account.UserName));
            });

        }

        [Test]
        public async Task WhenIncorrectAccountIdIsGiven_ReturnErrorResponse()
        {
            var result = await _accountService.GetAccountById(11, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.IsFailure, Is.True);
                Assert.That(result.Error, Is.EqualTo(AccountErrors.NotFound));
            });
        }


    }
}
