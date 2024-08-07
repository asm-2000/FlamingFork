using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class UserRegistrationPage : ContentPage
{
	public UserRegistrationPage()
	{
		InitializeComponent();
		BindingContext = new UserRegistrationViewModel(Navigation);
	}
}
