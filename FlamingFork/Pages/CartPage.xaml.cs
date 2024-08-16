using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class CartPage : ContentPage
{
	public CartPage()
	{
		InitializeComponent();
		BindingContext = new CartViewModel();
	}
}