using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FlamingFork.ViewModels
{
    public partial class InternetConnectionErrorViewModel: ObservableObject
    {
        private INavigation _Navigation;
        public InternetConnectionErrorViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }

        [RelayCommand]
        public async Task CheckInternetConnection()
        {
            NetworkAccess networkAccess = Connectivity.Current.NetworkAccess;
            if(networkAccess == NetworkAccess.Internet)
            {
                await _Navigation.PopAsync();
            }
        }
    }
}
