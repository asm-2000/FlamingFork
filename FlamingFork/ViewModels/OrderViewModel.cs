using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace FlamingFork.ViewModels
{
    public partial class OrderViewModel: ObservableObject
    {
        [ObservableProperty]
        private string _IsFetching;

        [ObservableProperty]
        private string _HasFetched;

        [ObservableProperty]
        private string _OrdersNotPresent;

        [ObservableProperty]
        private string _OrdersPresent;

        public OrderViewModel()
        {
            _IsFetching = "False";
            _HasFetched = "True";
            _OrdersNotPresent = "False";
        }

        [RelayCommand]
        public void ChangeOrderVisibilityByType()
        {
            Debug.WriteLine("Command Called!");
        }
    }
}
