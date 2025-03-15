using SuperMarket.Core.Interfaces;
using SuperMarket.DataAccess.Data;
using SuperMarket.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SuperMarket.Services
{
    /// <summary>
    /// Provides authentication functionalities for user registration and login.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes AuthService with the database context.
        /// </summary>
        /// <param name="context">Application DbContext</param>
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user into the system.
        /// </summary>
        /// <param name="user">User object that contains the details</param>
        /// <returns>Indicates if the registration was successful</returns>
        public async Task<bool> RegisterUserAsync(User user)
        {
            if (await UserExistsAsync(user.Email))
            {
                return false; // User already exists
            }

            // Hash password before storing it
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Logs in an existing user based on email and password.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>The user object if the login was successful, null otherwise.</returns>
        public async Task<User?> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user; // Return user if password matches
            }

            return null;
        }

        /// <summary>
        /// Checks if user with email already exists.
        /// </summary>
        /// <param name="email">User email to check</param>
        /// <returns>Indicates if user exists</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}