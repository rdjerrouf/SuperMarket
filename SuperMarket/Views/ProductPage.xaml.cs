using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    /// <summary>
    /// Code behind for ProductPage
    /// </summary>
    public partial class ProductPage : ContentPage
    {
        /// <summary>
        /// Initializes the product page
        /// </summary>
        /// <param name="viewModel"></param>
        public ProductPage(ProductViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}