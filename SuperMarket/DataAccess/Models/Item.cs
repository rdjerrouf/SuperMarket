namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents an item listing in the marketplace.
    /// </summary>
    public class Item
    {
        public int Id { get; set; } // Primary key for the item
        public string Title { get; set; } = String.Empty;// Title of the item
        public string Description { get; set; } = String.Empty; // Description of the item
        public decimal Price { get; set; } // Price of the item
        public string Category { get; set; } = String.Empty;// Category of the item (e.g., For Sale, Jobs)
        public string? PhotoUrl { get; set; } = String.Empty;// Optional URL for the item's photo
        public int UserId { get; set; } // Foreign key to the user who posted the item
        public User User { get; set; } = new();// Navigation property to the user
    }
}