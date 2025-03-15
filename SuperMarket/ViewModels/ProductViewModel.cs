using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.Core.Interfaces;
using SuperMarket.DataAccess.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for the ProductPage. Manages data and logic for product management.
    /// </summary>
    public partial class ProductViewModel : ObservableObject
    {
        private readonly IProductService _productService; // Instance of ProductService for data operations
                                                          // Backing properties for binding
        private string _title = string.Empty; // Backing field for product title
        private string _description = string.Empty; // Backing field for product description
        private decimal _price; // Backing field for product price
        private string _category = string.Empty; // Backing field for product category
        private string _message = string.Empty; // Backing field for messages to the UI
        private ObservableCollection<Product> _products = new(); // Backing field for product list
        private Product? _selectedProduct; // Backing field for the selected product
        private bool _isEditMode; // Indicates if product edition is active
        private bool _isLoading;

        /// <summary>
        ///  Indicates if the product list is being loaded from the database
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// Gets or sets the product title
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        /// <summary>
        /// Gets or sets the product description
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        /// <summary>
        /// Gets or sets the product category
        /// </summary>
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
        /// <summary>
        /// Gets or sets the messages to display in the UI
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// Gets or sets the product list to display
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        /// <summary>
        /// Gets or sets if we are editing the product
        /// </summary>
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }
        /// <summary>
        /// Gets or sets the selected product from the UI
        /// </summary>
       // Property with custom logic on set
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetProperty(ref _selectedProduct, value))
                {
                    if (value != null)
                    {
                        IsEditMode = true;
                        Title = value.Name;
                        Description = value.Description;
                        Price = value.Price;
                        Category = value.Category;
                    }
                    else
                    {
                        IsEditMode = false;
                        ClearForm();
                    }
                }
            }
        }

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            LoadProductsCommand.ExecuteAsync(null);
        }

        // Commands using partial methods from MVVM Toolkit
        [RelayCommand]
        private async Task LoadProducts()
        {
            IsLoading = true;
            var products = await _productService.GetProductsAsync();
            Products = new ObservableCollection<Product>(products);
            IsLoading = false;
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            if (!ValidateInput()) return;

            var product = new Product
            {
                Name = Title,
                Description = Description,
                Price = Price,
                Category = Category,
                // Set a default stock value
                StockQuantity = 0
            };

            if (await _productService.AddProductAsync(product))
            {
                Message = "Product added successfully";
                ClearForm();
                await LoadProductsCommand.ExecuteAsync(null);
            }
            else
            {
                Message = "Error adding product";
            }
        }

        [RelayCommand]
        private async Task UpdateProduct()
        {
            if (SelectedProduct == null || !ValidateInput()) return;

            SelectedProduct.Name = Title;
            SelectedProduct.Description = Description;
            SelectedProduct.Price = Price;
            SelectedProduct.Category = Category;

            if (await _productService.UpdateProductAsync(SelectedProduct))
            {
                Message = "Product updated successfully";
                ClearForm();
                SelectedProduct = null;
                await LoadProductsCommand.ExecuteAsync(null);
            }
            else
            {
                Message = "Error updating product";
            }
        }

        [RelayCommand]
        private async Task DeleteProduct()
        {
            if (SelectedProduct == null) return;

            if (await _productService.DeleteProductAsync(SelectedProduct.Id))
            {
                Message = "Product deleted successfully";
                ClearForm();
                SelectedProduct = null;
                await LoadProductsCommand.ExecuteAsync(null);
            }
            else
            {
                Message = "Error deleting product";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Title) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Category))
            {
                Message = "Please fill in all fields";
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            Price = 0;
            Category = string.Empty;
        }
    }
}