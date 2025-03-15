using SuperMarket.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarket.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartItemsAsync(int userId);
        Task<bool> AddItemToCartAsync(int userId, int productId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int cartItemId);
        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity);
        Task ClearCartAsync(int userId);
    }
}