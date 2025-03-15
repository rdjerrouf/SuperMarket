using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}