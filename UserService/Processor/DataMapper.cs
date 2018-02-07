//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     Called by the data repository to Map Data-Transfer-Objects (DTOs) to Domain (business in memory) objects.
//     Mapping is done here with the option to customize.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Services.User.Processors
{
    using AutoMapper;
    using Domain = Models.Domain;
    using Dto = Data.Repository.Models.DTO;

    public class DataMapper
    {
        private IMapper _mapper;

        public DataMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Domain.User, Dto.User>().ReverseMap();

                cfg.CreateMap<Domain.Address, Dto.Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.StateProvinceId, opt => opt.Ignore())
                .ReverseMap();

                cfg.CreateMap<Domain.ContactType, Dto.Contact>()
                .ForMember(dest => dest.Value, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ContactTypeId, opt => opt.MapFrom(src => src.Contact))
                .ReverseMap();

                cfg.CreateMap<Domain.ContactDetail, Dto.Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ContactTypeId, opt => opt.MapFrom(src => (int)src.Type.Contact))
                .ReverseMap();

                cfg.CreateMap<Domain.ContactType, Dto.ContactType>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Contact));
                cfg.CreateMap<Dto.ContactType, Domain.ContactType>().ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<Domain.StateProvince, Dto.StateProvince>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

                cfg.CreateMap<Domain.Preference, Dto.Preference>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PreferenceTypeId, opt => opt.MapFrom(src => (int)src.Type.Category))
                .ReverseMap();

            });

            config.AssertConfigurationIsValid();

            _mapper  = config.CreateMapper();
        }

        public Dto.User MapDomainUserToDtoUser(Domain.User User)
        {
            var UserDto = _mapper.Map<Dto.User>(User);

            return UserDto;
        }

        public List<Dto.Address> MapDomainAddressesToDtoAddress(List<Domain.Address> addresses)
        {
            var addressDto = _mapper.Map<List<Dto.Address>>(addresses);

            return addressDto;
        }


        public List<Dto.Contact> MapDomainContactDetailsToDtoContact(List<Domain.ContactDetail> contactDetails)
        {
            var contactDto = _mapper.Map<List<Dto.Contact>>(contactDetails);

            return contactDto;
        }

        public Dto.StateProvince MapDomainStateProvinceToDtoStateProvince(Domain.StateProvince stateProvince)
        {
            var stateProvinceDtos = _mapper.Map<Dto.StateProvince>(stateProvince);

            return stateProvinceDtos;
        }


        public List<Dto.Preference> MapDomainPreferencesToDtoPreferences(List<Domain.Preference> preferences)
        {
            var preferenceDtos = _mapper.Map<List<Dto.Preference>>(preferences);

            return preferenceDtos;
        }


        public List<Dto.Note> MapDomainNotesToDtoNotes(List<string> notes)
        {
            var dtoNotes = new List<Dto.Note>();
            notes.ForEach(note =>
            {
                var dtoNote = new Dto.Note();
                dtoNote.Notes = note;
                dtoNotes.Add(dtoNote);
            });

            return dtoNotes;
        }


        public Domain.User MapDtoUserToDomainUser(Dto.User User)
        {
            var domainUser = _mapper.Map<Domain.User>(User);
            return domainUser;
        }


        public List<Domain.Address> MapDtoAddressesToDomainAddresses(List<Dto.Address> addresses)
        {
            var domainAddress = _mapper.Map<List<Domain.Address>>(addresses);
            return domainAddress;
        }

        public List<Domain.ContactDetail> MapDtoContactsToDomainContactDetails(List<Dto.Contact> contactDetails)
        {
            var contactDetail = _mapper.Map<List<Domain.ContactDetail>>(contactDetails);
            return contactDetail;
        }

        public List<Domain.Preference> MapDtoPreferencesToDomainPreferences(List<Dto.Preference> preferences)
        {
            var domainPreferences = _mapper.Map<List<Domain.Preference>>(preferences);
            return domainPreferences;
        }

        public Domain.StateProvince MapDtoStateProvinceToDomainStateProvince(Dto.StateProvince stateProvince)
        {
            var domainStateProvince = _mapper.Map<Domain.StateProvince>(stateProvince);
            return domainStateProvince;
        }


        public List<string> MapDtoNotesToDomainNotes(List<Dto.Note> notes)
        {
            var domainNotes = new List<string>();
            notes.ForEach(note =>
            {
                domainNotes.Add(note.Notes);
            });

            return domainNotes;
        }
    }
}
