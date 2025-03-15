namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents an item within an order.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; } // Primary key for the order item
        public int OrderId { get; set; } // Foreign key to the order
        public int ProductId { get; set; } // Foreign key to the product
        public int Quantity { get; set; } // Quantity of the product in the order
        public decimal Price { get; set; } // Price of the product at the time of ordering

        // Navigation properties
        public Order Order { get; set; } = new(); // Order this item belongs to
        public Product Product { get; set; } = new(); // Product in the order
    }
}