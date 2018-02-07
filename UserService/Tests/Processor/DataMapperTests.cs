using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.User.Processors.Tests
{
    using AutoFixture;
    using FluentAssertions;

    using Domain = Processors.Models.Domain;
    using Dto = Data.Repository.Models.DTO;

    public class DataMapperTests
    {
        [TestClass]
        public class UserProcessorTests
        {
            private static DataMapper _dataMapper;

            private IFixture _fixture;

            [ClassInitialize()]
            public static void ClassInit(TestContext context)
            {
                _dataMapper = new DataMapper();
            }

            [TestInitialize]
            public void Initialize()
            {
                //_fixture = new Fixture().Customize(new MultipleCustomization());
                _fixture = new Fixture();
            }

            [TestCleanup]
            public void CleanUp()
            {
            }


            [TestMethod]
            public void MapDomainUserToDtoUser_Mapped()
            {
                // Arrange
                var User = _fixture.Create<Domain.User>();

                // Act
                Dto.User UserDto = _dataMapper.MapDomainUserToDtoUser(User);

                // Assert
                UserDto.Id.Should().Be(User.Id);
                UserDto.UserId.Should().Be(User.UserId);
                UserDto.TenantId.Should().Be(User.TenantId);

                UserDto.FirstName.Should().Be(User.FirstName);
                UserDto.MiddleName.Should().Be(User.MiddleName);
                UserDto.LastName.Should().Be(User.LastName);

                UserDto.Title.Should().Be(User.Title);
                UserDto.Created.Should().Be(User.Created);
                UserDto.Modified.Should().Be(User.Modified);
            }


            [TestMethod]
            public void MapDomainAddressToDtoAddress_Mapped()
            {
                // Arrange
                var addresses = _fixture.CreateMany<Domain.Address>(3).ToList();

                // Act
                var addressDtos = _dataMapper.MapDomainAddressesToDtoAddress(addresses);

                // Assert
                for (int i = 0; i < addressDtos.Count; i++)
                {
                    addressDtos[i].ShouldBeEquivalentTo(addresses[i], cfg =>
                    cfg.Excluding(s => s.Id)
                    .Excluding(s => s.UserId)
                    .Excluding(s => s.StateProvinceId));
                }
            }


            [TestMethod]
            public void MapDomainContactDetailsToDtoContact_Mapped()
            {
                // Arrange
                var contactDetails = _fixture.CreateMany<Domain.ContactDetail>(3).ToList();

                // Act
                var contactDtos = _dataMapper.MapDomainContactDetailsToDtoContact(contactDetails);

                // Assert
                for (int i = 0; i < contactDtos.Count; i++)
                {
                    contactDtos[i].ShouldBeEquivalentTo(contactDetails[i], cfg =>
                    cfg.Excluding(s => s.Id)
                    .Excluding(s => s.UserId)
                    .Excluding(s => s.ContactTypeId));

                    contactDtos[i].ContactTypeId.Should().Be((int)contactDetails[i].Type.Contact);
                }
            }


            [TestMethod]
            public void MapDomainPreferencesToDtoPreferences_Mapped()
            {
                // Arrange
                var preferences = _fixture.CreateMany<Domain.Preference>(3).ToList();

                // Act
                var preferenceDtos = _dataMapper.MapDomainPreferencesToDtoPreferences(preferences);

                // Assert
                for (int i = 0; i < preferenceDtos.Count; i++)
                {
                    preferenceDtos[i].ShouldBeEquivalentTo(preferences[i], cfg =>
                    cfg.Excluding(s => s.Id)
                    .Excluding(s => s.UserId)
                    .Excluding(s => s.PreferenceTypeId));

                    preferenceDtos[i].PreferenceTypeId.Should().Be((int)preferences[i].Type.Category);
                }
            }


            [TestMethod]
            public void MapDomainNotesToDtoNotes_Mapped()
            {
                // Arrange
                var notes = _fixture.CreateMany<string>(3).ToList();

                // Act
                var noteDtos = _dataMapper.MapDomainNotesToDtoNotes(notes);

                // Assert
                for (int i = 0; i < noteDtos.Count; i++)
                {
                    noteDtos[i].Notes.ShouldBeEquivalentTo(notes[i]);
                }
            }


            [TestMethod]
            public void MapDtoUserToDomainUser_Mapped()
            {
                // Arrange
                var dtoUser = _fixture.Create<Dto.User>();

                // Act
                Domain.User domainUser = _dataMapper.MapDtoUserToDomainUser(dtoUser);

                // Assert
                domainUser.Id.Should().Be(dtoUser.Id);
                domainUser.UserId.Should().Be(dtoUser.UserId);
                domainUser.TenantId.Should().Be(dtoUser.TenantId);

                domainUser.FirstName.Should().Be(dtoUser.FirstName);
                domainUser.MiddleName.Should().Be(dtoUser.MiddleName);
                domainUser.LastName.Should().Be(dtoUser.LastName);

                domainUser.Title.Should().Be(dtoUser.Title);
                domainUser.Created.Should().Be(dtoUser.Created);
                domainUser.Modified.Should().Be(dtoUser.Modified);
            }


            [TestMethod]
            public void MapDtoAddressesToDomainAddresses_Mapped()
            {
                // Arrange
                var dtoAddresses = _fixture.CreateMany<Dto.Address>(3).ToList();

                // Act
                var domainAddresses = _dataMapper.MapDtoAddressesToDomainAddresses(dtoAddresses);

                // Assert
                for (int i = 0; i < domainAddresses.Count; i++)
                {
                    domainAddresses[i].ShouldBeEquivalentTo(dtoAddresses[i], cfg =>
                    cfg.Excluding(s => s.StateProvince));
                }
            }

            [TestMethod]
            public void MapDtoContactsToDomainContactDetails_Mapped()
            {
                // Arrange
                var dtoContacts = _fixture.CreateMany<Dto.Contact>(3).ToList();

                // Act
                var contactDetails = _dataMapper.MapDtoContactsToDomainContactDetails(dtoContacts);

                // Assert
                for (int i = 0; i < dtoContacts.Count; i++)
                {
                    contactDetails[i].ShouldBeEquivalentTo(dtoContacts[i], cfg =>
                    cfg.Excluding(s => s.Type));
                }
            }


            [TestMethod]
            public void MapDtoPreferencesToDomainPreferences_Mapped()
            {
                // Arrange
                var dtoPreferences = _fixture.CreateMany<Dto.Preference>(3).ToList();

                // Act
                var domainPreferences = _dataMapper.MapDtoPreferencesToDomainPreferences(dtoPreferences);

                // Assert
                for (int i = 0; i < dtoPreferences.Count; i++)
                {
                    domainPreferences[i].ShouldBeEquivalentTo(dtoPreferences[i]);
                }
            }


            [TestMethod]
            public void MapDtoNotesToDomainNotes_Mapped()
            {
                // Arrange
                var dtoNotes = _fixture.CreateMany<Dto.Note>(3).ToList();

                // Act
                var domainNotes = _dataMapper.MapDtoNotesToDomainNotes(dtoNotes);

                // Assert
                for (int i = 0; i < dtoNotes.Count; i++)
                {
                    domainNotes[i].ShouldBeEquivalentTo(dtoNotes[i].Notes);
                }
            }


        }
    }

}

