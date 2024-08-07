using CommunityToolkit.Mvvm.ComponentModel;

namespace FlamingFork.ViewModels
{
    public partial class UserLoginViewModel: ObservableObject
    {
        private INavigation _Navigation;

        public UserLoginViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }
    }
}
