using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.ApiServices;

namespace FlamingFork.ViewModels
{
    public partial class UserDetailsViewModel: ObservableObject
    {
        [ObservableProperty]
        private CustomerModel _CustomerDetails;

        [ObservableProperty]
        private string _NewContact;

        [ObservableProperty]
        private string _NewAddress;

        [ObservableProperty]
        private string _ShowDetailsUI;

        [ObservableProperty]
        private string _ShowUpdateUI;

        [ObservableProperty]
        private string _ContactNumberError;

        [ObservableProperty]
        private string _AddressError;

        [ObservableProperty]
        private string _IsUpdating;

        [ObservableProperty]
        private string _UpdateMessage;

        [ObservableProperty]
        private string _UpdateMessageVisibility;

        private AuthenticationServiceRepository _AuthenticationService;

        public UserDetailsViewModel()
        {
            _AuthenticationService = new AuthenticationServiceRepository();
            ShowUpdateUI = "False";
            UpdateMessageVisibility = "False";
            ShowDetailsUI = "True";
            GetCustomerDetails();
        }

        [RelayCommand]
        public async Task GetCustomerDetails()
        {
            CustomerDetails = await SecureStorageHandler.GetCustomerDetails();
            // Assign the old values to new values for assisted editing.
            NewContact = CustomerDetails.Contact;
            NewAddress = CustomerDetails.Address;
        }

        [RelayCommand]
        public void MakeUpdateUIVisible()
        {
            ShowUpdateUI = "True";
            ShowDetailsUI = "False";
        }

        [RelayCommand]
        public void ValidateContactNumber()
        {
            ContactNumberError = Validation.ContactNumberValidator(NewContact);
        }

        [RelayCommand]
        public void ValidateAddress()
        {
            AddressError = Validation.AddressValidator(NewAddress);
        }

        [RelayCommand]
        public void CancelUpdate()
        {
            ShowDetailsUI = "True";
            ShowUpdateUI = "False";
        }

        [RelayCommand]
        public async Task UpdateCustomerDetails()
        {
            IsUpdating = "True";
            CustomerModel updatedDetails = new(CustomerDetails.CustomerID, CustomerDetails.CustomerName,NewAddress,NewContact);
            UpdateMessage = await _AuthenticationService.UpdateCustomerDetails(updatedDetails);
            CustomerDetails = await SecureStorageHandler.GetCustomerDetails();
            IsUpdating = "False";
            ShowDetailsUI = "True";
            ShowUpdateUI = "False";
            UpdateMessageVisibility = "True";
            await Task.Delay(1000);
            UpdateMessageVisibility = "False";
        }
    }
}
