using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.DataAccess.Models;
using SuperMarket.Core.Interfaces;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for the Registration page with expression-bodied properties
    /// </summary>
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        // Backing fields
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _message = string.Empty;
        private bool _isLoading;

        // Expression-bodied properties
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public RegistrationViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Message = "Please fill in all fields";
                return;
            }

            if (Password != ConfirmPassword)
            {
                Message = "Passwords do not match";
                return;
            }

            IsLoading = true;
            Message = string.Empty;

            try
            {
                var userExists = await _authService.UserExistsAsync(Email);

                if (userExists)
                {
                    Message = "Email is already registered";
                    return;
                }

                var user = new User
                {
                    Email = Email,
                    PasswordHash = Password // The service will hash it
                };

                var result = await _authService.RegisterUserAsync(user);

                if (result)
                {
                    Message = "Registration successful";
                    // Either navigate to login or directly to main page based on your flow
                    await Shell.Current.GoToAsync("//SignInPage");
                }
                else
                {
                    Message = "Registration failed";
                }
            }
            catch (Exception ex)
            {
                Message = $"Registration error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}