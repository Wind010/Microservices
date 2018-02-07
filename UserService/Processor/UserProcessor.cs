//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     Acts as a second repository that interacts between domain objects and the data repository.
//     Can relay events/messages to interfaces depending on need.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Services.User.Processors.Tests")]


namespace Services.User.Processors
{
    using Data.Repository;
    using Domain = Models.Domain;
    using Dto = Data.Repository.Models.DTO;

    using NLog;

    public class UserProcessor : IUserProcessor
    {
        private IUserRepository _UserRepository;
        private DataMapper _dataMapper;
        private ILogger _logger;

        public UserProcessor(IUserRepository UserRepository, ILogger logger)
        {
            _UserRepository = UserRepository;
            _logger = logger;
            _dataMapper = new DataMapper();
        }

        public async Task<int> AddUser(Domain.User User)
        {
            Dto.User dtoUser = _dataMapper.MapDomainUserToDtoUser(User);
            List<Dto.Address> dtoAddress = _dataMapper.MapDomainAddressesToDtoAddress(User.Addresses);
            List<Dto.Contact> dtoContacts = _dataMapper.MapDomainContactDetailsToDtoContact(User.ContactDetails);
            List<Dto.Preference> dtoPreferences = _dataMapper.MapDomainPreferencesToDtoPreferences(User.Preferences);
            List<Dto.Note> dtoNotes = _dataMapper.MapDomainNotesToDtoNotes(User.Notes);

            // TODO:  StateProvince and ContactType lookups.
            // Can optimize by T-SQL directly.

            for(int i=0; i < User.Addresses.Count; i++)
            {
                string stateCode = User.Addresses[i].StateProvince.Code;
                Dto.StateProvince stateProvince = _UserRepository.GetStateProvinceByCodeAsync(stateCode).Result;
                dtoAddress[i].StateProvinceId = stateProvince.Id;
            }

            dtoUser.Created = DateTime.Now;

            User.Id = await _UserRepository.AddUserAsync(dtoUser, dtoAddress, dtoContacts, dtoPreferences, dtoNotes);

            return User.Id;
        }

    

        internal void AssociateStateProvinceIdToAddress(List<Domain.Address> domainAddressses, List<Dto.Address> dtoAddress)
        {
            for (int i = 0; i < domainAddressses.Count; i++)
            {
                string stateCode = domainAddressses[i].StateProvince.Code;
                Dto.StateProvince stateProvince = _UserRepository.GetStateProvinceByCodeAsync(stateCode).Result;
                dtoAddress[i].StateProvinceId = stateProvince.Id;
            }
        }


        public async Task<Domain.User> GetUserById(int UserId)
        {
            Dto.User dtoUser = await _UserRepository.GetUserAsync(UserId);
            Domain.User User = _dataMapper.MapDtoUserToDomainUser(dtoUser);

            List<Dto.Address> dtoAddresses = await _UserRepository.GetAddressesByUserIdAsync(UserId);
            List<Domain.Address> addresses = _dataMapper.MapDtoAddressesToDomainAddresses(dtoAddresses);
            User.Addresses = addresses;

            List<Dto.Contact> dtoContacts = await _UserRepository.GetContactsByUserIdAsync(UserId);
            List<Domain.ContactDetail> contactDetails = _dataMapper.MapDtoContactsToDomainContactDetails(dtoContacts);
            User.ContactDetails = contactDetails;

            List<Dto.Preference> dtoPreferences = await _UserRepository.GetPreferencesByUserIdAsync(UserId);
            List<Domain.Preference> preferences = _dataMapper.MapDtoPreferencesToDomainPreferences(dtoPreferences);
            User.Preferences = preferences;

            List<Dto.Note> dtoNotes = await _UserRepository.GetNotesByUserIdAsync(UserId);
            List<string> notes = _dataMapper.MapDtoNotesToDomainNotes(dtoNotes);
            User.Notes = notes;

            return User;
        }



    }

}
