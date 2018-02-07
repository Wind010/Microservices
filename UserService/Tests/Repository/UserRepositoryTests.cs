
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;

namespace Services.User.Data.Repository.Tests.Integration
{
    using Models.DTO;

    using AutoFixture;
    using FluentAssertions;


    [TestClass]
    public class UserProcessorTests
    {
        private IFixture _fixture;
        private static UserRepository _userRepository;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Locati‌​on))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            IConfiguration config = builder.Build();

            //string connectionString = config["connectionStrings:UserProfileDb"];
            string connectionString = config.GetConnectionString("UserProfileDb");

            _userRepository = new UserRepository(connectionString);
        }

        [TestInitialize]
        public void Initialize()
        {
            var _dtoGenerator = new DtoGenerator();
            _fixture = _dtoGenerator.Fixture;
        }

        [TestCleanup]
        public void CleanUp()
        {
            _userRepository.DeleteAllUsersAndDetails().Wait();
        }

        [TestMethod]
        public void AddUserAsync_UserAddressesContactsPreferencesNotes_Added()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var addresses = new List<Address>() { (_fixture.Create<Address>()) };
            var contacts = new List<Contact>() { (_fixture.Create<Contact>()) };
            var preferences = new List<Preference>() { (_fixture.Create<Preference>()) };
            var notes = new List<Note>() { (_fixture.Create<Note>()) };

            // Act
            int userId = _userRepository.AddUserAsync(user, addresses, contacts, preferences, notes).Result;

            // Assert
            userId.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void AddUserAsync_UserMultipleAddressesContactsPreferencesNotes_Added()
        {
            var user = _fixture.Create<User>();
            var addresses = _fixture.CreateMany<Address>(3).ToList();
            var contacts = _fixture.CreateMany<Contact>(3).ToList();
            var preferences = _fixture.CreateMany<Preference>(1).ToList();
            var notes = _fixture.CreateMany<Note>(5).ToList();

            // Act
            int userId = _userRepository.AddUserAsync(user, addresses, contacts, preferences, notes).Result;

            // Assert
            userId.Should().BeGreaterThan(0);
        }


        [TestMethod]
        public void GetUserAsync_UserExists_UserReturned()
        {
            var generatedUser = _fixture.Create<User>();
            int userId = _userRepository.AddUserAsync(generatedUser).Result;

            // Act
            var user = _userRepository.GetUserAsync(userId).Result;

            // Assert
            userId.Should().BeGreaterThan(0);
            
            user.ShouldBeEquivalentTo(generatedUser,
                cfg => cfg.Excluding(s => s.Id)
                .Using<DateTime>(c => c.Subject.Should()
                .BeCloseTo(user.Created.Value)).WhenTypeIs<DateTime>());
        }


        [TestMethod]
        public void GetAddressesByUserIdAsync_AddressesExists_AddressReturned()
        {
            const int count = 2;
            var generatedUser = _fixture.Create<User>();
            var generatedAddress = _fixture.CreateMany<Address>(count).ToList();
            int UserId = _userRepository.AddUserAsync(generatedUser, generatedAddress, null, null, null).Result;
            
            // Act
            var addresses = _userRepository.GetAddressesByUserIdAsync(UserId).Result;

            // Assert
            UserId.Should().BeGreaterThan(0);
            addresses.Count.Should().Be(count);
            for(int i=0; i < addresses.Count; i++)
            {
                addresses[i].ShouldBeEquivalentTo(generatedAddress[i],
                cfg => cfg.Excluding(s => s.Id)
                .Using<DateTime>(c => c.Subject.Should()
                .BeCloseTo(generatedAddress[i].Created.Value)).WhenTypeIs<DateTime>());
            }
        }


        [TestMethod]
        public void GetContactsByUserIdAsync_ContactsExists_ContactsReturned()
        {
            const int count = 3;
            var generatedUser = _fixture.Create<User>();
            var generatedContacts = _fixture.CreateMany<Contact>(count).ToList();
            int UserId = _userRepository.AddUserAsync(generatedUser, null, generatedContacts, null, null).Result;

            // Act
            var contacts = _userRepository.GetContactsByUserIdAsync(UserId).Result;

            // Assert
            UserId.Should().BeGreaterThan(0);
            contacts.Count.Should().Be(count);
            for (int i = 0; i < contacts.Count; i++)
            {
                contacts[i].ShouldBeEquivalentTo(generatedContacts[i], cfg => cfg.Excluding(s => s.Id));
            }
        }


        [TestMethod]
        public void GetPreferencesByUserIdAsync_PreferencesExists_PreferencesReturned()
        {
            const int count = 3;
            var generatedUser = _fixture.Create<User>();
            var generatedPreferences = _fixture.CreateMany<Preference>(count).ToList();
            int UserId = _userRepository.AddUserAsync(generatedUser, null, null, generatedPreferences, null).Result;

            // Act
            var preferences = _userRepository.GetPreferencesByUserIdAsync(UserId).Result;

            // Assert
            UserId.Should().BeGreaterThan(0);
            preferences.Count.Should().Be(count);
            for (int i = 0; i < preferences.Count; i++)
            {
                preferences[i].ShouldBeEquivalentTo(generatedPreferences[i], cfg => cfg.Excluding(s => s.Id));
            }
        }


        [TestMethod]
        public void GetNotesByUserIdAsync_NotesExists_AddressReturned()
        {
            const int count = 2;
            var generatedUser = _fixture.Create<User>();
            var generatedNotes = _fixture.CreateMany<Note>(count).ToList();
            int userId = _userRepository.AddUserAsync(generatedUser, null, null, null, generatedNotes).Result;

            // Act
            var notes = _userRepository.GetNotesByUserIdAsync(userId).Result;

            // Assert
            userId.Should().BeGreaterThan(0);
            notes.Count.Should().Be(count);
            for (int i = 0; i < notes.Count; i++)
            {
                notes[i].ShouldBeEquivalentTo(generatedNotes[i], cfg => cfg.Excluding(s => s.Id));
            }
        }

    }
}
