using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class OrderPage : ContentPage
{
	private OrderViewModel _OrderViewModel;
	public OrderPage(OrderViewModel orderViewModel)
	{
		InitializeComponent();
		_OrderViewModel = orderViewModel;
		BindingContext = orderViewModel;
	}
}
