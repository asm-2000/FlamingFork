using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;
using FlamingFork.Pages;

namespace FlamingFork.ViewModels
{
    public partial class UserLoginViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private string _Email;

        [ObservableProperty]
        private string _Password;

        [ObservableProperty]
        private string _EmailError;

        [ObservableProperty]
        private string _PasswordError;

        private bool _FormValidity;

        private INavigation _Navigation;

        #endregion Properties

        public UserLoginViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _FormValidity = false;
            _EmailError = "";
            _PasswordError = "";
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
        public async Task ValidateForm()
        {
            _FormValidity = string.IsNullOrEmpty(EmailError = Validation.EmailValidator(Email)) && string.IsNullOrEmpty(PasswordError = Validation.PasswordValidator(Password));
            if (_FormValidity)
            {
                await _Navigation.PopModalAsync();
            }
        }

        #endregion Validation Methods

        [RelayCommand]
        public async Task CreateAnAccount()
        {
            await _Navigation.PushModalAsync(new UserRegistrationPage());
        }
    }
}
