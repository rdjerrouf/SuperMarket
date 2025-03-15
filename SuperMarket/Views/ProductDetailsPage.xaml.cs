using SuperMarket.ViewModels;

namespace SuperMarket.Views
{
    public partial class ProductDetailsPage : ContentPage
    {
        public ProductDetailsPage(ProductDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}