using SuperMarket.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarket.Services //Correct namespace, based on our discussion
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, List<CartItem> cartItems);
        Task<List<Order>> GetOrdersAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}