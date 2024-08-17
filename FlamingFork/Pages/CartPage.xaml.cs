using FlamingFork.ViewModels;

namespace FlamingFork.Pages;

public partial class CartPage : ContentPage
{
    private readonly CartViewModel _CartViewModel;
    public CartPage(CartViewModel cartViewModel)
    {
        InitializeComponent();
        _CartViewModel = cartViewModel;
        BindingContext = cartViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _CartViewModel.FetchCartItems();
    }
}
