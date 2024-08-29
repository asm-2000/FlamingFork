using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Models;
using FlamingFork.Repositories.ApiServices;
using System.Diagnostics;

namespace FlamingFork.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private List<CustomerOrderModel> _AllCustomerOrders;

        [ObservableProperty]
        private string _IsFetching;

        [ObservableProperty]
        private string _HasFetched;

        [ObservableProperty]
        private string _OrdersNotPresent;

        [ObservableProperty]
        private string _OrdersPresent;

        [ObservableProperty]
        private string _CancelOrderResponse;

        [ObservableProperty]
        private string _ResponseVisibility;

        private OrderServiceRepository _OrderServices;

        #endregion Properties

        #region Constructor

        public OrderViewModel()
        {
            _IsFetching = "False";
            _HasFetched = "True";
            _OrdersNotPresent = "False";
            _OrdersPresent = "True";
            _ResponseVisibility = "False";
            _OrderServices = new OrderServiceRepository();
            FetchCustomerOrders();
        }

        #endregion Constructor

        #region Methods

        [RelayCommand]
        public async Task FetchCustomerOrders()
        {
            // Shows activity indicator while the app is fetching the customer orders.
            IsFetching = "True";
            HasFetched = "False";
            AllCustomerOrders = await _OrderServices.GetCustomerOrders();
            AllCustomerOrders =  AllCustomerOrders.OrderByDescending(order => order.OrderDate).ToList();
            // Check for empty order list and show proper UI.
            if(AllCustomerOrders.Count == 0)
            {
                OrdersNotPresent = "True";
                OrdersPresent = "False";
            }
            else
            {
                OrdersNotPresent = "False";
                OrdersPresent = "True";
            }
            // Change the flag values after sucessful fetch call.
            HasFetched = "True";
            IsFetching = "False";
        }

        [RelayCommand]
        public async Task CancelSpecificCustomerOrder(string orderId)
        {
            await FetchCustomerOrders();
            CustomerOrderModel? specificOrder = AllCustomerOrders.Find(order => Convert.ToString(order.OrderId) == orderId);
            CancelOrderResponse = await _OrderServices.CancelCustomerOrder(specificOrder);
            ResponseVisibility = "True";
            if(ConvertToBool(CancelOrderResponse))
            {
                ChangeOrderStatus(orderId);
            }
            await Task.Delay(500);
            ResponseVisibility = "False";
        }

        public void ChangeOrderStatus(string orderId)
        { 
            foreach(CustomerOrderModel customerOrder in AllCustomerOrders)
            {
                if(orderId == Convert.ToString(customerOrder.CustomerId))
                {
                    customerOrder.OrderStatus = "Cancelled";
                }
            }
        }

        public bool ConvertToBool(string response) => response == "Status updated sucessfully!";

        #endregion Methods
    }
}