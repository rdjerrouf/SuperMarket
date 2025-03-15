using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    /// <summary>
    /// Code behind for Cart Page
    /// </summary>
    public partial class CartPage : ContentPage
    {
        /// <summary>
        /// Initializes Cart page
        /// </summary>
        /// <param name="viewModel">Cart view model</param>
        public CartPage(CartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}