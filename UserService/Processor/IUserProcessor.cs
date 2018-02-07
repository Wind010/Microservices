//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Interface for UserProcessor 
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace Services.User.Processors
{
    using Domain = Models.Domain;

    public interface IUserProcessor
    {
        Task<int> AddUser(Domain.User User);

        Task<Domain.User> GetUserById(int UserId);

    }
}
