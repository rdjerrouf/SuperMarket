namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents a product in the marketplace.
    /// </summary>
    public class Product
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } = string.Empty; // Product name
        public string Description { get; set; } = string.Empty; // Product description
        public decimal Price { get; set; } // Product price
        public string Category { get; set; } = string.Empty; // Product category
        public string? ImageUrl { get; set; } // Optional image URL
        public int StockQuantity { get; set; } // Quantity in stock

        // Foreign key to the user who added the product
        public int UserId { get; set; }
        public User User { get; set; } = new(); // Navigation property
    }
}