namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents an item in the user's shopping cart.
    /// </summary>
    public class CartItem
    {
        public int Id { get; set; } // Primary key for the cart item
        public int ProductId { get; set; } // Foreign key to the product
        public int UserId { get; set; } // Foreign key to the user
        public int Quantity { get; set; } // Quantity of the product in the cart

        // Navigation properties
        public Product Product { get; set; } = new(); // Product in the cart
        public User User { get; set; } = new(); // User who added the product to the cart
    }
}