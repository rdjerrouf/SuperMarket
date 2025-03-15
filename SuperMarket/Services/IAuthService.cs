using SuperMarket.DataAccess.Models;
using System.Threading.Tasks;

namespace SuperMarket.Core.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> LoginUserAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
    }
}