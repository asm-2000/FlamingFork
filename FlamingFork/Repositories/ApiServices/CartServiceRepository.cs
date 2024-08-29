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
            _Address = "10.10.100.53:8080";
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
            ApiResponseMessageModal? errorResponse = new();
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

                var uri = new Uri("http://" + _Address + "/cart/userCart/" + customerId);
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
                    errorResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

        #region Cart Clearer

        public async Task<string> ClearCart()
        {
            ApiResponseMessageModal? apiResponse = new();
            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);

            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/cart/clearCartItems/" + customerId);
                var response = await _HttpClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return apiResponse.Message;
                }
                // Returns failure if status code is not ok.
                else
                {
                    return "Failed to clear your cart!";
                }
            }
            // Returns empty list if communication with API fails.
            catch
            {
                return "Failed: Could not communicate with server!";
            }
        }

        #endregion Cart Clearer

        #region Cart Item Deleter

        public async Task<string> DeleteSpecificCartItem(CartItemModel cartItem)
        {
            ApiResponseMessageModal? apiResponse = new();

            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);
            string details = customerId + " " + Convert.ToString(cartItem.CartItemId);
            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/cart/deleteCartItem/" + details);
                var response = await _HttpClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return apiResponse.Message;
                }
                // Returns failure if status code is not ok.
                else
                {
                    return "Failed to delete cart item!";
                }
            }
            // Returns empty list if communication with API fails.
            catch
            {
                return "Failed: Could not communicate with server!";
            }
        }

        #endregion Cart Item Deleter

        #region Cart Checkout Handler

        public async Task<string> CheckoutAndPlaceOrder(List<CartItemModel> cartItems, string customerCurrentAddress)
        {
            List<OrderItemModel> orderItems = new List<OrderItemModel>();
            ApiResponseMessageModal? apiResponse = new();
            // Extract necessary details about customer from secure storage.
            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);
            string? customerContact = currentCustomer.Contact;
            // Maps each cartitem to orderitem.
            foreach (CartItemModel cartItem in cartItems)
            {
                OrderItemModel orderItem = new(cartItem.CartItemName, cartItem.CartItemPrice, cartItem.Quantity);
                orderItems.Add(orderItem);
            }
            //Extract current time.
            DateTime currentDate = DateTime.Now;
            //Convert the current date to suitable string format.
            string orderDate = $"{currentDate.Date:yyyy/MM/dd} {currentDate.Hour}:{currentDate.Minute}";
            CustomerOrderModel customerOrder = new(customerId, customerContact, customerCurrentAddress, "Placed", orderItems, orderDate);
            // Json serialization.
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonContent = JsonSerializer.Serialize<CustomerOrderModel>(customerOrder, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/order/placeCustomerOrder");
                var response = await _HttpClient.PostAsync(uri, content);

                // Tries to deserialize the response to ApiResponseMessageModel in case of sucessful order placement.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return apiResponse.Message;
                }
                else
                {
                    return "Failed to place order!";
                }
            }
            // Returns exception message if communication with API fails.
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Cart Checkout Handler
    }
}
