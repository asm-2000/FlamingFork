using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;
using FlamingFork.Models;

namespace FlamingFork.ViewModels
{
    public partial class UserDetailsViewModel: ObservableObject
    {
        [ObservableProperty]
        private CustomerModel _CustomerDetails;

        public UserDetailsViewModel()
        {
            GetCustomerDetails();
        }

        [RelayCommand]
        public async Task GetCustomerDetails()
        {
            CustomerDetails = await SecureStorageHandler.GetCustomerDetails();
        }
    }
}
