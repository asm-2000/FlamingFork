using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class InternetConnectionErrorPage : ContentPage
{
	public InternetConnectionErrorPage()
	{
		InitializeComponent();
		BindingContext = new InternetConnectionErrorViewModel(Navigation);
	}
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}
