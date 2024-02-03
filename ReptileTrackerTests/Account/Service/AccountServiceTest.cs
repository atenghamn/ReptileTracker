using NSubstitute;
using ReptileTracker.Infrastructure.Persistence;
using ReptileTracker.Account.Service;

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
                LastName = "Doe"
            };

            var ct = new CancellationToken();

            _mockedAccountRepository.GetByIdAsync(1, ct).Returns(account);

            var result = await _accountService.GetAccountById(1, ct);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data?.Id, Is.EqualTo("1"));
                Assert.That(result.Data?.FirstName, Is.EqualTo("Jane"));
                Assert.That(result.Data?.LastName, Is.EqualTo("Doe"));
            });

        }


    }
}
