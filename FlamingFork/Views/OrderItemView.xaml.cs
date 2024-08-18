using System.Windows.Input;

namespace FlamingFork.Views;

public partial class OrderItemView : ContentView
{
    // OrderId Bindable Property
    public static readonly BindableProperty OrderIdProperty =
    BindableProperty.Create(nameof(OrderId), typeof(int), typeof(OrderItemView), 0);

    public int OrderId
    {
        get => (int)GetValue(OrderIdProperty);
        set => SetValue(OrderIdProperty, value);
    }

    // Delivery Address Bindable Property
    public static readonly BindableProperty DeliveryAddressProperty =
    BindableProperty.Create(nameof(DeliveryAddress), typeof(string), typeof(OrderItemView), string.Empty);

    public string DeliveryAddress
    {
        get => (string)GetValue(DeliveryAddressProperty);
        set => SetValue(DeliveryAddressProperty, value);
    }

    // Items Bindable Property
    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(string), typeof(OrderItemView), string.Empty);

    public string Items
    {
        get => (string)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    // TotalPrice Bindable Property
    public static readonly BindableProperty TotalPriceProperty =
        BindableProperty.Create(nameof(TotalPrice), typeof(int), typeof(OrderItemView), 0);

    public string OrderStatus
    {
        get => (string)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    // OrderStatus Bindable Property
    public static readonly BindableProperty OrderStatusProperty =
        BindableProperty.Create(nameof(OrderStatus), typeof(int), typeof(OrderItemView), 0);

    public int TotalPrice
    {
        get => (int)GetValue(TotalPriceProperty);
        set => SetValue(TotalPriceProperty, value);
    }

    public static readonly BindableProperty CancelButtonCommandProperty =
    BindableProperty.Create(nameof(CancelButtonCommand), typeof(ICommand), typeof(OrderItemView));

    public ICommand CancelButtonCommand
    {
        get => (ICommand)GetValue(CancelButtonCommandProperty);
        set => SetValue(CancelButtonCommandProperty, value);
    }

    public OrderItemView()
    {
        InitializeComponent();
    }
}
