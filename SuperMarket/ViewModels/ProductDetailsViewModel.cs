using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Core.Interfaces;
using SuperMarket.Services;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for product details page with expression-bodied properties
    /// </summary>
    [QueryProperty(nameof(ProductId), "Id")]
    public partial class ProductDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        // Backing fields
        private Product _product = new();
        private int _productId;
        private int _quantity = 1;
        private string _message = string.Empty;
        private bool _isLoading;
        private int _userId = 1; // TODO: Replace with actual user ID from auth system

        // Expression-bodied properties
        public Product Product { get => _product; set => SetProperty(ref _product, value); }
        public int ProductId { get => _productId; set => SetProperty(ref _productId, value); }
        public int Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public bool IsNotLoading { get => !_isLoading; }

        public ProductDetailsViewModel(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out var id))
            {
                ProductId = Convert.ToInt32(id);
                LoadProductCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task LoadProduct()
        {
            if (ProductId == 0) return;

            IsLoading = true;
            Message = string.Empty;

            try
            {
                var product = await _productService.GetProductByIdAsync(ProductId);

                if (product != null)
                {
                    Product = product;
                }
                else
                {
                    Message = "Product not found";
                    // Navigate back or show error
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Message = $"Error loading product: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }

        [RelayCommand]
        private void IncreaseQuantity()
        {
            if (Product.StockQuantity > Quantity)
            {
                Quantity++;
            }
            else
            {
                Message = "Maximum available quantity reached";
            }
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
            }
        }

        [RelayCommand]
        private async Task AddToCart()
        {
            if (Product == null || Product.Id == 0) return;

            IsLoading = true;
            Message = string.Empty;

            try
            {
                var result = await _cartService.AddItemToCartAsync(_userId, Product.Id, Quantity);

                if (result)
                {
                    Message = $"{Quantity} {Product.Name} added to cart";
                    // Reset quantity after adding to cart
                    Quantity = 1;
                }
                else
                {
                    Message = "Error adding to cart";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }
    }
}