using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class UserDetailsPage : ContentPage
{
	public UserDetailsPage()
	{
		InitializeComponent();
		BindingContext = new UserDetailsViewModel();
	}
}
