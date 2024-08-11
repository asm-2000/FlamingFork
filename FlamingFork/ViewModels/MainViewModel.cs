using CommunityToolkit.Mvvm.ComponentModel;
using FlamingFork.Pages;

namespace FlamingFork.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private INavigation _Navigation;
        public MainViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _Navigation.PushModalAsync(new UserLoginPage());
        }
    }
}
