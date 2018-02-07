//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     The data repository that takes in data-transfer-objects which model the database and performs 
//     operations on that persistent data store. 
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Services.User.Data.Repository.Tests.Integration")]

namespace Services.User.Data.Repository
{
    using Common.BaseRepository;

    using Models.DTO;

    using Dapper;
    using Dapper.Contrib.Extensions;


    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
            // TODO:  Add nLog;
        }


        public async Task<int> AddUserAsync(User User, List<Address> addresses, List<Contact> contacts, List<Preference> preferences, List<Note> notes)
        {
            // TODO:  Try when released: https://github.com/dotnet/corefx/issues/24282

            //using (var trans = new TransactionScope())
            //{ 
            int rowsInserted = await WithConnectionAsync(async c =>
                {
                    int UserId = 0;

                    UserId = await c.InsertAsync(User);
                    User.Id = UserId;

                    await AddAddressesWithUserId(UserId, c, addresses);

                    await AddContactsWithUserId(UserId, c, contacts);

                    await AddPreferencesWithUserId(UserId, c, preferences);

                    await AddNotesWithUserId(UserId, c, notes);

                    return UserId;
                });

                //trans.Complete();

                return rowsInserted;
            //}
        }



        #region User

        public async Task<int> AddUserAsync(User User)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(User);
            });
        }

        public async Task<bool> UpdateUserAsync(User User)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<User>(User);
            });
        }

        public async Task<bool> DeleteUserAsync(User User)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<User>(User);
            });
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                var User = await c.GetAsync<User>(id);
                User.Title = User.Title.Trim();

                return User;
            });
        }

        #endregion User


        #region Address

        public async Task<int> AddAddressAsync(Address address)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(address);
            });
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<Address>(address);
            });
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<Address>(address);
            });
        }

        public async Task<Address> GetAddressAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<Address>(id);
            });
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int UserId)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM Address WHERE UserId = @UserId";
                var addresses = await c.QueryAsync<Address>(sql, new { UserId = UserId });

                foreach(Address address in addresses)
                {
                    address.ZipCode = address.ZipCode.Trim();
                }

                return addresses.ToList();
            });
        }

        #endregion Address


        #region Preferences

        public async Task<int> AddPreferenceAsync(Preference preference)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(preference);
            });
        }

        public async Task<bool> UpdatePreferenceAsync(Preference preference)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<Preference>(preference);
            });
        }

        public async Task<bool> DeletePreferenceAsync(Preference preference)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<Preference>(preference);
            });
        }

        public async Task<Preference> GetPreferenceAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<Preference>(id);
            });
        }

        public async Task<List<Preference>> GetPreferencesByUserIdAsync(int UserId)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM Preference WHERE UserId = @UserId";
                return (await c.QueryAsync<Preference>(sql, new { UserId = UserId })).ToList();
            });
        }

        #endregion Preferences


        #region Interfaces

        public async Task<int> AddInterfaceAsync(Interface myInterface)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(myInterface);
            });
        }

        public async Task<bool> UpdateInterfaceAsync(Interface myInterface)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<Interface>(myInterface);
            });
        }

        public async Task<bool> DeleteInterfaceAsync(Interface myInterface)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<Interface>(myInterface);
            });
        }

        public async Task<Interface> GetInterfaceAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<Interface>(id);
            });
        }

        #endregion Interfaces


        #region Contact

        public async Task<int> AddContactAsync(Contact Contact)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(Contact);
            });
        }

        public async Task<bool> UpdateContactAsync(Contact Contact)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<Contact>(Contact);
            });
        }

        public async Task<bool> DeleteContactAsync(Contact Contact)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<Contact>(Contact);
            });
        }

        public async Task<Contact> GetContactAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<Contact>(id);
            });
        }


        public async Task<List<Contact>> GetContactsByUserIdAsync(int UserId)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM Contact WHERE UserId = @UserId";
                return (await c.QueryAsync<Contact>(sql, new { UserId = UserId })).ToList();
            });
        }

        #endregion Contact


        #region Notes

        public async Task<int> AddNoteAsync(Note note)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.InsertAsync(note);
            });
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.UpdateAsync<Note>(note);
            });
        }

        public async Task<bool> DeleteNoteAsync(Note note)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.DeleteAsync<Note>(note);
            });
        }

        public async Task<Note> GetNoteAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<Note>(id);
            });
        }

        public async Task<List<Note>> GetNotesByUserIdAsync(int UserId)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM Note WHERE UserId = @UserId";
                return (await c.QueryAsync<Note>(sql, new { UserId = UserId })).ToList();
            });
        }

        #endregion Notes


        #region StateProvince

        public async Task<StateProvince> GetStateProvinceByIdAsync(int id)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.GetAsync<StateProvince>(id);
            });
        }

        public async Task<List<StateProvince>> GetAllStateProvinces()
        {
            return await WithConnectionAsync(async c =>
            {
                return (await c.GetAllAsync<StateProvince>()).ToList();
            });
        }
        
        public async Task<StateProvince> GetStateProvinceByCodeAsync(string code)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM StateProvince WHERE Code = @Code";
                return (await c.QueryAsync<StateProvince>(sql, new { Code = code })).FirstOrDefault();
            });
        }


        public async Task<StateProvince> GetStateProvinceByNameAsync(string name)
        {
            return await WithConnectionAsync(async c =>
            {
                const string sql = "SELECT * FROM StateProvince WHERE Name = @Name";
                return (await c.QueryAsync<StateProvince>(sql, new { Name = name })).FirstOrDefault();
            });
        }

        #endregion StateProvince



        internal async Task<int> AddAddressesWithUserId(int UserId, IDbConnection connection, List<Address> addresses)
        {
            int rowsInserted = 0;

            if (addresses == null)
            {
                return rowsInserted;
            }

            foreach (Address address in addresses)
            {
                address.UserId = UserId;
                long addressId = await connection.InsertAsync(address);
                address.Id = (int)addressId;
                rowsInserted++;
            };

            return rowsInserted;
        }

        internal async Task<int> AddContactsWithUserId(int UserId, IDbConnection connection, List<Contact> contacts)
        {
            int rowsInserted = 0;

            if (contacts == null)
            {
                return rowsInserted;
            }

            foreach (Contact contact in contacts)
            {
                contact.UserId = UserId;
                long contactId = await connection.InsertAsync(contact);
                contact.Id = (int)contactId;
                rowsInserted++;
            };

            return rowsInserted;
        }

        internal async Task<int> AddPreferencesWithUserId(int UserId, IDbConnection connection, List<Preference> preferences)
        {
            int rowsInserted = 0;

            if (preferences == null)
            {
                return rowsInserted;
            }

            foreach (Preference preference in preferences)
            {
                preference.UserId = UserId;
                long preferenceId = await connection.InsertAsync(preference);
                preference.Id = (int)preferenceId;
                rowsInserted++;
            };

            return rowsInserted;
        }

        internal async Task<int> AddNotesWithUserId(int UserId, IDbConnection connection, List<Note> Notes)
        {
            int rowsInserted = 0;

            if (Notes == null)
            {
                return rowsInserted;
            }

            foreach (Note note in Notes)
            {
                note.UserId = UserId;
                long preferenceId = await connection.InsertAsync(note);
                note.Id = (int)preferenceId;
                rowsInserted++;
            };

            return rowsInserted;
        }

        

        /// <summary>
        /// Delete all Users, addresses, contacts, preferences, and notes.
        /// </summary>
        /// <returns><see cref="Task{int}"/>The rows deleted.</returns>
        internal async Task<int> DeleteAllUsersAndDetails()
        {
            return await WithConnectionAsync(async c =>
            {
                string sqlResetIdentity = @"DBCC CHECKIDENT('[Note]', RESEED, 0);
                    DBCC CHECKIDENT('[Preference]', RESEED, 0);DBCC CHECKIDENT('[Contact]', RESEED, 0)
                    DBCC CHECKIDENT('[Address]', RESEED, 0);DBCC CHECKIDENT('[User]', RESEED, 0)";

                await c.ExecuteAsync(sqlResetIdentity);

                int rows = await c.ExecuteAsync("TRUNCATE TABLE Note");
                rows += await c.ExecuteAsync("TRUNCATE TABLE Preference");
                rows += await c.ExecuteAsync("TRUNCATE TABLE Contact");
                rows += await c.ExecuteAsync("TRUNCATE TABLE Address");

                return await c.ExecuteAsync("DELETE FROM User") + rows;
            });
        }

    }
}
