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
            string customerDetails = JsonSerializer.Serialize(customer);
            await SecureStorage.SetAsync("Token", authToken);
            await SecureStorage.SetAsync("CustomerDetails", customerDetails);
        }

        #endregion Token Storage Handler

        #region Token Returner

        public static async Task<string> GetAuthenticationToken()
        {
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
            string? customer = await SecureStorage.GetAsync("CustomerDetails");
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
