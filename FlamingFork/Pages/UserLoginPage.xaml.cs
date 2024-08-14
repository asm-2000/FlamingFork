using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class UserLoginPage : ContentPage
{
	public UserLoginPage()
	{
		InitializeComponent();
		BindingContext = new UserLoginViewModel(Navigation);
	}
	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}
