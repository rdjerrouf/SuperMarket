using Microsoft.EntityFrameworkCore;
using SuperMarket.DataAccess.Models;

namespace SuperMarket.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        // DbSet properties represent database tables
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } //Added OrderItems DbSet


        // Constructor to configure the database context
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between Product and User
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User) // A product is posted by one user
                .WithMany(u => u.Products) // A user can post many products
                .HasForeignKey(p => p.UserId); // Foreign key is UserId

            // Configure the relationship between Order and User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User) // An order belongs to one user
                .WithMany(u => u.Orders) // A user can have many orders
                .HasForeignKey(o => o.UserId); // Foreign key is UserId

            // Configure the relationship between CartItem and User
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User) // A cart item belongs to one user
                .WithMany(u => u.CartItems) // A user can have many cart items
                .HasForeignKey(c => c.UserId); // Foreign key is UserId

            // Configure the relationship between CartItem and Product
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product) // A cart item references one product
                .WithMany() // A product can be in many cart items
                .HasForeignKey(c => c.ProductId); // Foreign key is ProductId
        }
    }
}