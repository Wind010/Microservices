

using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.User.Processors.Tests
{
    using Data.Repository;

    using Domain = Processors.Models.Domain;
    using Dto = Data.Repository.Models.DTO;

    using AutoFixture;
    using FluentAssertions;
    using Moq;

    [TestClass]
    public class UserProcessorTests
    {
        private IFixture _fixture;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            // TODO:  Add domainGenerator.  Abstract generation out to separate library.
            _fixture = new Fixture();
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void AssociateStateProvinceIdToAddress_AddressesAsociated()
        {
            // Arrange
            var stateProvince = new Dto.StateProvince()
            {
                Id = 54,
                Code = "WA",
                Name = "Washington",
                Country = "United States"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetStateProvinceByCodeAsync(stateProvince.Code)).Returns(Task.FromResult(stateProvince));
            var UserProcessor = new UserProcessor(mockUserRepository.Object, null);

            // https://github.com/AutoFixture/AutoFixture/issues/724
            //var domainAddresses = _fixture.Build<Domain.Address>()
            //    .With(x => x.StateProvince.Code, stateProvince.Code).CreateMany(3).ToList();

            var domainAddresses = _fixture.Build<Domain.Address>().CreateMany().ToList();
            domainAddresses.ForEach(address => { address.StateProvince.Code = stateProvince.Code; });

            var dtoAddresses = _fixture.Build<Dto.Address>()
                .With(x => x.StateProvinceId, 0).CreateMany(3).ToList();

            // Act
            UserProcessor.AssociateStateProvinceIdToAddress(domainAddresses, dtoAddresses);

            // Assert
            dtoAddresses.ForEach(address => { address.StateProvinceId.Should().Be(stateProvince.Id); });
        }


 

    }

}
    
