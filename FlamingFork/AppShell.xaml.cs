using FlamingFork.Pages;
using FlamingFork.ViewModels;

namespace FlamingFork
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel(Navigation);
            
            Routing.RegisterRoute(nameof(UserLoginPage), typeof(UserLoginPage));
            Routing.RegisterRoute(nameof(UserRegistrationPage), typeof(UserRegistrationPage));
        }
    }
}
