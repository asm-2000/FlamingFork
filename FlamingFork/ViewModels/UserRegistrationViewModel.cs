using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;

namespace FlamingFork.ViewModels
{
    public partial class UserRegistrationViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private string? _FullName;

        [ObservableProperty]
        private string? _Email;

        [ObservableProperty]
        private string? _Password;

        [ObservableProperty]
        private string? _Address;

        [ObservableProperty]
        private string? _ContactNumber;

        [ObservableProperty]
        private string? _EmailError;

        [ObservableProperty]
        private string? _PasswordError;

        [ObservableProperty]
        private string? _ContactNumberError;

        #endregion Properties

        private INavigation _Navigation;

        public UserRegistrationViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }

        #region Validation Methods

        [RelayCommand]
        public void ValidateEmail()
        {
            EmailError = Validation.EmailValidator(Email);
        }

        [RelayCommand]
        public void ValidatePassword()
        {
            PasswordError = Validation.PasswordValidator(Password);
        }

        [RelayCommand]
        public void ValidateContactNumber()
        {
            ContactNumberError = Validation.ContactNumberValidator(ContactNumber);
        }

        #endregion Validation Methods

        [RelayCommand]
        public async Task RegisterAccount()
        {
            if (string.IsNullOrEmpty(EmailError) && string.IsNullOrEmpty(PasswordError))
            {
                await _Navigation.PopModalAsync();
            }
        }
    }
}
