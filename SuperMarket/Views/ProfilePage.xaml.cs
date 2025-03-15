using SuperMarket.ViewModels;
using Microsoft.Maui.Controls;

namespace SuperMarket.Views
{
    /// <summary>
    /// Code behind for Profile Page
    /// </summary>
    public partial class ProfilePage : ContentPage
    {
        /// <summary>
        /// Initializes Profile Page
        /// </summary>
        /// <param name="viewModel">Profile View Model</param>
        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}