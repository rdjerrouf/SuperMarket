using SuperMarket.Core.Interfaces;
using SuperMarket.DataAccess.Data;
using SuperMarket.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMarket.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            return await _context.CartItems
                                 .Include(ci => ci.Product)
                                 .Where(ci => ci.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<bool> AddItemToCartAsync(int userId, int productId, int quantity)
        {
            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task ClearCartAsync(int userId)
        {
            var cartItemsToRemove = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(cartItemsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}