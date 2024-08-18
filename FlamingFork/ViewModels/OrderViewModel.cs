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

        private OrderServiceRepository _OrderServices;

        #endregion Properties

        #region Constructor

        public OrderViewModel()
        {
            _IsFetching = "False";
            _HasFetched = "True";
            _OrdersNotPresent = "False";
            _OrdersPresent = "True";
            _OrderServices = new OrderServiceRepository();
            FetchCustomerOrders();
        }

        #endregion Constructor

        #region Methods

        [RelayCommand]
        public void ChangeOrderVisibilityByType()
        {
            Debug.WriteLine("Command Called!");
        }

        [RelayCommand]
        public async Task FetchCustomerOrders()
        {
            AllCustomerOrders = await _OrderServices.GetCustomerOrders();
        }

        #endregion Methods
    }
}