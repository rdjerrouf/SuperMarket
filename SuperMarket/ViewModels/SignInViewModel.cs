using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Core.Interfaces;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for the Sign In page with expression-bodied properties
    /// </summary>
    public partial class SignInViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        // Backing fields
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _message = string.Empty;
        private bool _isLoading;

        // Expression-bodied properties
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public SignInViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Please enter both email and password";
                return;
            }

            IsLoading = true;
            Message = string.Empty;

            try
            {
                var user = await _authService.LoginUserAsync(Email, Password);

                if (user != null)
                {
                    // TODO: Store user info in a session service
                    Message = "Login successful";
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    Message = "Invalid email or password";
                }
            }
            catch (Exception ex)
            {
                Message = $"Login error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync("//RegistrationPage");
        }
    }
}