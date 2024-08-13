using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;
using FlamingFork.Pages;

namespace FlamingFork.ViewModels
{
    public partial class AppShellViewModel: ObservableObject
    {
        private INavigation _Navigation;
        public AppShellViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }

        [RelayCommand]
        public async Task LogOut()
        {
            // Clears all the saved data.
            SecureStorageHandler.ClearStorage();
            // Closes Flyout.
            Shell.Current.FlyoutIsPresented = false;
            // Clears Shell's navigation stack.
            Shell.Current.Items.Clear();
            // Initialized the app to its initial state.
            App.Current.MainPage = new AppShell();
            // Opens Sign in page.
            await _Navigation.PushModalAsync(new UserLoginPage());
        }
    }
}
