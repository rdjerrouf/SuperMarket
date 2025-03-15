using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    /// <summary>
    /// Code behind for Order Page.
    /// </summary>
    public partial class OrderPage : ContentPage
    {
        /// <summary>
        /// Initializes Order page
        /// </summary>
        /// <param name="viewModel">Order view model</param>
        public OrderPage(OrderViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}