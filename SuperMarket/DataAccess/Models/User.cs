namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        public int Id { get; set; } // Primary key for the user
        public string Email { get; set; } = string.Empty; // User's email address (unique)
        public string PasswordHash { get; set; } = string.Empty; // Hashed password for security
        public string? Location { get; set; } // Optional location data
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for when the user was created

        // Navigation properties
        public List<Product> Products { get; set; } = new(); // Products posted by the user
        public List<Order> Orders { get; set; } = new(); // Orders placed by the user
        public List<CartItem> CartItems { get; set; } = new(); // Items in the user's cart
    }
}