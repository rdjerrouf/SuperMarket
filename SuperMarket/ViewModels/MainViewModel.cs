using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Core.Interfaces;
using SuperMarket.Services;
using System.Collections.ObjectModel;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for the main product listing page with expression-bodied properties
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        // Backing fields
        private ObservableCollection<Product> _items = new();
        private string _searchQuery = string.Empty;
        private bool _isLoading;
        private Product? _selectedProduct;
        private string _message = string.Empty;
        private int _userId = 1; // TODO: Replace with actual user ID from auth system

        // Expression-bodied properties
        public ObservableCollection<Product> Items { get => _items; set => SetProperty(ref _items, value); }
        public string SearchQuery { get => _searchQuery; set => SetProperty(ref _searchQuery, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public MainViewModel(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;

            LoadProductsCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            IsLoading = true;
            Message = string.Empty;

            try
            {
                var products = await _productService.GetProductsAsync();
                Items = new ObservableCollection<Product>(products);
                Message = Items.Count > 0 ? $"{Items.Count} products found" : "No products available";
            }
            catch (Exception ex)
            {
                Message = $"Error loading products: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await LoadProductsCommand.ExecuteAsync(null);
                return;
            }

            IsLoading = true;
            Message = string.Empty;

            try
            {
                var products = await _productService.SearchProductsAsync(SearchQuery);
                Items = new ObservableCollection<Product>(products);
                Message = Items.Count > 0 ? $"{Items.Count} products found" : "No products match your search";
            }
            catch (Exception ex)
            {
                Message = $"Error searching products: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddToCart(Product product)
        {
            if (product == null) return;

            try
            {
                await _cartService.AddItemToCartAsync(_userId, product.Id, 1);
                Message = $"{product.Name} added to cart";
            }
            catch (Exception ex)
            {
                Message = $"Error adding to cart: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ViewProductDetails(Product product)
        {
            if (product == null) return;

            // Navigate to product details page with the product ID
            var parameters = new Dictionary<string, object>
            {
                { "Id", product.Id }
            };

            await Shell.Current.GoToAsync("ProductDetailsPage", parameters);
        }

        [RelayCommand]
        private async Task GoToProfile()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        [RelayCommand]
        private async Task GoToCart()
        {
            await Shell.Current.GoToAsync("//CartPage");
        }
    }
}