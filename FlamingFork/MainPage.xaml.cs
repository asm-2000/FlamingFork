using FlamingFork.ViewModels;

namespace FlamingFork
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);
        }
    }
}
