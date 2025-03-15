using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SuperMarket.Core.Interfaces;

namespace SuperMarket.ViewModels
{
    /// <summary>
    /// ViewModel for the Profile page with expression-bodied properties
    /// </summary>
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        // Backing fields
        private string _email = string.Empty;
        private string _location = string.Empty;
        private string _message = string.Empty;
        private bool _isLoading;

        // Expression-bodied properties
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Location { get => _location; set => SetProperty(ref _location, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public ProfileViewModel(IAuthService authService)
        {
            _authService = authService;

            // TODO: Replace with actual user data from a session service
            Email = "user@example.com";
            Location = "Test Location";
            LoadUserDataCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadUserData()
        {
            IsLoading = true;

            try
            {
                // Simulate API call with a delay
                await Task.Delay(500);

                // TODO: Implement loading user data from a service
                // For now using placeholder data
                Email = "user@example.com";

                // In a real app, we'd get this from the database
                // var user = await _userService.GetCurrentUserAsync();
                // Email = user.Email;
                // Location = user.Location;
            }
            catch (Exception ex)
            {
                Message = $"Error loading profile: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            IsLoading = true;
            try
            {
                // Simulate an asynchronous operation until the actual implementation
                await Task.Delay(500);

                // TODO: Implement updating user profile
                // var result = await _userService.UpdateUserProfileAsync(Email, Location);
                Message = "Profile updated successfully";
            }
            catch (Exception ex)
            {
                Message = $"Error updating profile: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private async Task Logout()
        {
            // TODO: Implement logout functionality (clear session/token)
            await Shell.Current.GoToAsync("SignInPage");
        }
    }
}