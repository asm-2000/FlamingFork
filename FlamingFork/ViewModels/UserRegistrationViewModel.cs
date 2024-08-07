using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FlamingFork.ViewModels
{
    public partial class UserRegistrationViewModel: ObservableObject
    {
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

        private INavigation _Navigation;

        public UserRegistrationViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }

        [RelayCommand]
        public async Task RegisterAccount()
        {
            await _Navigation.PopAsync();
        }
    }
}
