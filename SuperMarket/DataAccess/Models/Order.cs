namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents an order placed by a user.
    /// </summary>
    public class Order
    {
        public int Id { get; set; } // Primary key for the order
        public int UserId { get; set; } // Foreign key to the user who placed the order
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Timestamp for when the order was placed
        public decimal TotalAmount { get; set; } // Total amount of the order
        public string Status { get; set; } = "Pending"; // Status of the order (e.g., Pending, Shipped, Delivered)

        // Navigation properties
        public User User { get; set; } = new(); // User who placed the order
        public List<OrderItem> OrderItems { get; set; } = new(); // Items in the order
    }
}