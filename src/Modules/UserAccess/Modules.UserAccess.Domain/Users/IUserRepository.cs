using System.Threading.Tasks;

namespace Modules.UserAccess.Domain.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}