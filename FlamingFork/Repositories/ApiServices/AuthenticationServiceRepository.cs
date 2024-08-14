using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Text.Json;
using System.Text;
using FlamingFork.Helper.Utilities;
using System.Diagnostics;

namespace FlamingFork.Repositories.ApiServices
{
    public class AuthenticationServiceRepository : IAuthenticationServiceRepository
    {
        private string _Address;
        private HttpClient _HttpClient;

        public AuthenticationServiceRepository()
        {
            _HttpClient = new HttpClient();
            _Address = "10.10.100.111:8080";
        }

        #region SignUp Handler API Service

        public async Task<string> RegisterCustomer(CustomerModel customerDetails)
        {
            ApiResponseMessageModal? registerResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize<CustomerModel>(customerDetails, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                var uri = new Uri("http://" + _Address + "/user/registerCustomer");
                var response = await _HttpClient.PostAsync(uri, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                registerResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return registerResponse.Message;
            }
            // Returns exception message if communication with API fails.
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion SignUp Handler API Service

        #region Login Handler API Service

        public async Task<string> LoginCustomer(CustomerLoginModel customerCredentials)
        {
            LoginResponseModel? loginResponse = new();
            ApiResponseMessageModal? loginErrorResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize<CustomerLoginModel>(customerCredentials, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                var uri = new Uri("http://" + _Address + "/user/loginCustomer");
                var response = await _HttpClient.PostAsync(uri, content);

                // Tries to deserialize the response to LoginResponseModel in case of sucessful login.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonSerializer.Deserialize<LoginResponseModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    await SecureStorageHandler.StoreAuthenticationToken(loginResponse);
                    return "Sign In successful!";
                }
                // Deserializes the response to ApiResponseMessageModel in case of error status.
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    loginErrorResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return loginErrorResponse.Message;
                }
            }
            // Returns exception message if communication with API fails.
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Login Handler API Service

        #region Menu Items Fetcher

        public async Task<List<CartItemModel>> GetMenuItems()
        {
            List<MenuItemModel>? fetchedMenuItems = [];
            List<CartItemModel>? menuItems = [];
            ApiResponseMessageModal? ErrorResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            try
            {
                var uri = new Uri("http://" + _Address + "/menu/allMenuItems");
                var response = await _HttpClient.GetAsync(uri);

                // Tries to deserialize the response to List<MenuItemModel> in case of sucessful fetch.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    fetchedMenuItems = JsonSerializer.Deserialize<List<MenuItemModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    // Fetch customerId from secure storage.
                    CustomerModel customerDetails = await SecureStorageHandler.GetCustomerDetails();
                    int customerId = Convert.ToInt16(customerDetails.CustomerID);
                    // Maps details from MenuItemModel to CartItemModel for simplifying add to cart
                    // functionality in Main Page

                    foreach (MenuItemModel menuitem in fetchedMenuItems)
                    {
                        CartItemModel correspodingCartItem = new(customerId, menuitem.ItemName, menuitem.ItemPrice, 1);
                        menuItems.Add(correspodingCartItem);
                    }

                    return menuItems;
                }
                // Deserializes the response to ApiResponseMessageModel in case of error status.
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ErrorResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return [];
                }
            }
            // Returns empty list if communication with API fails.
            catch
            {
                return [];
            }
        }

        #endregion Menu Items Fetcher
    }
}
