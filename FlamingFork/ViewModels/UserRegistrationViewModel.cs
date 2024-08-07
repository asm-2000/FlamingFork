using CommunityToolkit.Mvvm.ComponentModel;

namespace FlamingFork.ViewModels
{
    public partial class UserRegistrationViewModel: ObservableObject
    {
        private INavigation _Navigation;

        public UserRegistrationViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }
    }
}
