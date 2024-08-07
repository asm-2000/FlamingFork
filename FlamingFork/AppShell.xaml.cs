using FlamingFork.Pages;

namespace FlamingFork
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UserLoginPage), typeof(UserLoginPage));
            Routing.RegisterRoute(nameof(UserRegistrationPage), typeof(UserRegistrationPage));
        }
    }
}
