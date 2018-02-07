
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Services.User.Data.Repository
{
    using Models.DTO;

    public interface IUserRepository
    {
        Task<int> AddUserAsync(User User, List<Address> addresses, List<Contact> contacts, 
            List<Preference> preferences, List<Note> notes);

        Task<User> GetUserAsync(int id);

        Task<List<Address>> GetAddressesByUserIdAsync(int UserId);

        Task<List<Preference>> GetPreferencesByUserIdAsync(int UserId);

        Task<List<Contact>> GetContactsByUserIdAsync(int UserId);

        Task<List<Note>> GetNotesByUserIdAsync(int UserId);
        

        Task<List<StateProvince>> GetAllStateProvinces();


        Task<StateProvince> GetStateProvinceByIdAsync(int id);

        Task<StateProvince> GetStateProvinceByCodeAsync(string code);

        Task<StateProvince> GetStateProvinceByNameAsync(string name);
    }
}
