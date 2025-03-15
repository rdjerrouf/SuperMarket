using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage(SignInViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}