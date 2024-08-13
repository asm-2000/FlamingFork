using FlamingFork.Models;
using System.Text;
using System.Text.Json;

namespace FlamingFork.Helper.Utilities
{
    public static class SecureStorageHandler
    {
        #region Token Storage Handler

        public static async void StoreAuthenticationToken(LoginResponseModel loginResponse)
        {
            string? authToken = loginResponse.Token;
            CustomerModel? customer = loginResponse.CustomerDetails;
            // Serializes the customer details for storage.
            string customerDetails = JsonSerializer.Serialize(customer);
            //Store info in secure storage.
            await SecureStorage.SetAsync("Token", authToken);
            await SecureStorage.SetAsync("CustomerDetails", customerDetails);
        }

        #endregion Token Storage Handler

        #region Token Returner

        public static async Task<string> GetAuthenticationToken()
        {
            // Sees if the key-value pair exists.
            try
            {
                string? token = await SecureStorage.GetAsync("Token");
                return token;
            }
            catch
            {
                return "Not Found";
            }
        }

        #endregion Token Returner

        #region Customer Details returner

        public static async Task<CustomerModel> GetCustomerDetails()
        {
            // Fetches serialized data from secure storage.
            string? customer = await SecureStorage.GetAsync("CustomerDetails");
            // Deserializes the fetched string data into CustomerModel.
            CustomerModel? customerDetails = JsonSerializer.Deserialize<CustomerModel>(customer);
            return customerDetails;
        }

        #endregion Customer Details returner

        #region Storage Clearer

        public static async void ClearStorage()
        {
            SecureStorage.RemoveAll();
        }

        #endregion Storage Clearer
    }
}
