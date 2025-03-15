using System;

namespace SuperMarket.DataAccess.Models
{
    /// <summary>
    /// Represents a message between users.
    /// </summary>
    public class Message
    {
        public int Id { get; set; } // Primary key for the message
        public string Content { get; set; } = String.Empty; // Content of the message
        public int SenderId { get; set; } // Foreign key to the sender user
        public int ReceiverId { get; set; } // Foreign key to the receiver user
        public int? ProductId { get; set; } // Optional foreign key to the related product
        public bool IsRead { get; set; } = false; // Indicates if the message has been read
        public DateTime SentAt { get; set; } = DateTime.UtcNow; // Timestamp for when the message was sent

        // Navigation properties
        public User Sender { get; set; } = new User();// Sender of the message
        public User Receiver { get; set; } = new();// Receiver of the message
        public Product? Product { get; set; } = new();// Related product
    }
}