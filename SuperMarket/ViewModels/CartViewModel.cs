using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for shopping cart management with expression-bodied properties
    /// </summary>
    public partial class CartViewModel : ObservableObject
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        // Backing fields
        private ObservableCollection<CartItem> _cartItems = new();
        private string _message = string.Empty;
        private decimal _total;
        private bool _isLoading;
        private int _userId = 1; // TODO: Replace with actual user ID from auth system

        // Expression-bodied properties with backing fields
        public ObservableCollection<CartItem> CartItems { get => _cartItems; set => SetProperty(ref _cartItems, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public decimal Total { get => _total; set => SetProperty(ref _total, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public CartViewModel(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
            LoadCartItemsCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadCartItems()
        {
            IsLoading = true;
            try
            {
                var cartItems = await _cartService.GetCartItemsAsync(_userId);
                CartItems = new ObservableCollection<CartItem>(cartItems);
                CalculateTotal();
                Message = CartItems.Count > 0 ? $"{CartItems.Count} items in cart" : "Your cart is empty";
            }
            catch (Exception ex)
            {
                Message = $"Error loading cart: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task IncreaseQuantity(CartItem cartItem)
        {
            if (cartItem == null) return;

            var newQuantity = cartItem.Quantity + 1;
            if (await _cartService.UpdateCartItemQuantityAsync(cartItem.Id, newQuantity))
            {
                cartItem.Quantity = newQuantity;
                CalculateTotal();
            }
            else
            {
                Message = "Error updating quantity";
            }
        }

        [RelayCommand]
        private async Task DecreaseQuantity(CartItem cartItem)
        {
            if (cartItem == null || cartItem.Quantity <= 1) return;

            var newQuantity = cartItem.Quantity - 1;
            if (await _cartService.UpdateCartItemQuantityAsync(cartItem.Id, newQuantity))
            {
                cartItem.Quantity = newQuantity;
                CalculateTotal();
            }
            else
            {
                Message = "Error updating quantity";
            }
        }

        [RelayCommand]
        private async Task RemoveItem(CartItem cartItem)
        {
            if (cartItem == null) return;

            if (await _cartService.RemoveItemFromCartAsync(cartItem.Id))
            {
                CartItems.Remove(cartItem);
                CalculateTotal();
                Message = "Item removed from cart";
            }
            else
            {
                Message = "Error removing item";
            }
        }

        [RelayCommand]
        private async Task ClearCart()
        {
            if (CartItems.Count == 0) return;

            await _cartService.ClearCartAsync(_userId);
            CartItems.Clear();
            CalculateTotal();
            Message = "Cart cleared";
        }

        [RelayCommand]
        private async Task Checkout()
        {
            if (CartItems.Count == 0)
            {
                Message = "Your cart is empty";
                return;
            }

            IsLoading = true;
            try
            {
                var order = await _orderService.CreateOrderAsync(_userId, CartItems.ToList());
                if (order != null)
                {
                    await _cartService.ClearCartAsync(_userId);
                    CartItems.Clear();
                    CalculateTotal();
                    Message = $"Order #{order.Id} created successfully";
                    await Shell.Current.GoToAsync("//OrderPage");
                }
            }
            catch (Exception ex)
            {
                Message = $"Error creating order: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CalculateTotal()
        {
            Total = CartItems.Sum(item => item.Product.Price * item.Quantity);
        }
    }
}