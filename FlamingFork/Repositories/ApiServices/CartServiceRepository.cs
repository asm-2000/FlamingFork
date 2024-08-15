using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FlamingFork.Repositories.ApiServices
{
    public class CartServiceRepository : ICartServiceRepository
    {
        private HttpClient _HttpClient;
        private string _Address;

        public CartServiceRepository()
        {
            _HttpClient = new HttpClient();
            _Address = "192.168.10.72:8080";
        }

        #region Item Adder

        public async Task<string> AddItemToCart(CartItemModel cartItem)
        {
            ApiResponseMessageModal? ErrorResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize<CartItemModel>(cartItem, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/cart/saveCartItem");
                var response = await _HttpClient.PostAsync(uri, content);

                // Tries to deserialize the response to ApiResponseMessageModel in case of sucessful addition.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ErrorResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return ErrorResponse.Message;
                }
                else
                {
                    return "Failed to add item to cart!";
                }
            }
            // Returns exception message if communication with API fails.
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Item Adder

        #region Cart Items Fetcher

        public async Task<List<CartItemModel>> GetAllCartItems()
        {
            CartItemsModel? fetchedCartItems = new();
            ApiResponseMessageModal? ErrorResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);

            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/cart/allCartItems/"+customerId);
                var response = await _HttpClient.GetAsync(uri);

                // Tries to deserialize the response to List<CartItemModel> in case of sucessful fetch.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    fetchedCartItems = JsonSerializer.Deserialize<CartItemsModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return fetchedCartItems.AllCartItems;
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

        #endregion Cart Items Fetcher

        public async Task<string> ClearCart()
        {
            return "sucess";
        }
    }
}