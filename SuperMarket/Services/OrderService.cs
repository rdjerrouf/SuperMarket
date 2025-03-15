using SuperMarket.Core.Interfaces;
using SuperMarket.DataAccess.Data;
using SuperMarket.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMarket.Services
{
    /// <summary>
    /// Provides functionalities for Order management.
    /// Implements IOrderService.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes OrderService with a database context.
        /// </summary>
        /// <param name="context">Application DbContext</param>
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new order in the database using a list of cart items and a user id.
        /// </summary>
        /// <param name="userId">Id of the user placing the order.</param>
        /// <param name="cartItems">List of cart items for the order</param>
        /// <returns>Order object of the created order</returns>
        public async Task<Order> CreateOrderAsync(int userId, List<CartItem> cartItems)
        {
            // Create new order.
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };
            // Add order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order items to the database
            decimal totalAmount = 0;
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                totalAmount += cartItem.Product.Price * cartItem.Quantity;
                _context.OrderItems.Add(orderItem);
            }
            //update the order with total amount and persist data
            order.TotalAmount = totalAmount;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;

        }

        /// <summary>
        /// Gets all orders by user id
        /// </summary>
        /// <param name="userId">Id of the user placing the order.</param>
        /// <returns>List of Orders</returns>
        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific order by Id.
        /// </summary>
        /// <param name="orderId">Id of the order</param>
        /// <returns>Order object.</returns>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
              .Include(o => o.OrderItems)
              .ThenInclude(oi => oi.Product)
              .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}