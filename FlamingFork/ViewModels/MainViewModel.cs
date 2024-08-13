using CommunityToolkit.Mvvm.ComponentModel;
using FlamingFork.Helper.Utilities;
using FlamingFork.Pages;
using System.Diagnostics;

namespace FlamingFork.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private INavigation _Navigation;
        public MainViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            CheckLoginStatus();
        }

        // Checks if the user has previously logged in or not and navigates accordingly
        public async void CheckLoginStatus()
        {
            string? token = await SecureStorageHandler.GetAuthenticationToken();
            Action navigationAction = token == "Not Found" ? (() => { _Navigation.PushModalAsync(new UserLoginPage()); }) : (() => { FetchMenuData(); });
            navigationAction();
        }

        public void FetchMenuData()
        {
            Debug.WriteLine("fetched");
        }
    }
}
