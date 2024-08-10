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
        private string? _FullNameError;

        [ObservableProperty]
        private string? _EmailError;

        [ObservableProperty]
        private string? _PasswordError;

        [ObservableProperty]
        private string? _ContactNumberError;

        [ObservableProperty]
        private string? _AddressError;

        private bool _FormValidity;
        private INavigation _Navigation;

        #endregion Properties

        public UserRegistrationViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _FormValidity = false;
        }

        #region Validation Methods

        [RelayCommand]
        public void ValidateName()
        {
            FullNameError = Validation.NameValidator(FullName);
        }

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

        [RelayCommand]
        public void ValidateAddress()
        {
            AddressError = Validation.AddressValidator(Address);
        }

        #endregion Validation Methods

        [RelayCommand]
        public async Task RegisterAccount()
        {
            FullNameError = Validation.NameValidator(FullName);
            EmailError = Validation.EmailValidator(Email);
            PasswordError = Validation.PasswordValidator(Password);
            AddressError = Validation.AddressValidator(Address);
            ContactNumberError = Validation.ContactNumberValidator(ContactNumber);

            _FormValidity = string.IsNullOrEmpty(EmailError) && string.IsNullOrEmpty(PasswordError) && string.IsNullOrEmpty(FullNameError) && string.IsNullOrEmpty(ContactNumberError) && string.IsNullOrEmpty(AddressError);
            if (_FormValidity)
            {
                await _Navigation.PopModalAsync();
            }
        }
    }
}
