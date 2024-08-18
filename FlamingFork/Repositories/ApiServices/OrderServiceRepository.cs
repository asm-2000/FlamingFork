using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FlamingFork.Repositories.ApiServices
{
    public class OrderServiceRepository: IOrderServiceRepository
    {
        private HttpClient _HttpClient;
        private string _Address;
        public OrderServiceRepository()
        {
            _HttpClient = new HttpClient();
            _Address = "192.168.10.72:8080";
        }

        public async Task<List<CustomerOrderModel>> GetCustomerOrders()
        {
            List<CustomerOrderModel>? allCustomerOrders = new();
            ApiResponseMessageModal? errorResponse = new ();

            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);

            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/orders/customerOrders/" + customerId);
                var response = await _HttpClient.GetAsync(uri);

                // Tries to deserialize the response to List<CustomerOrderModel> in case of sucessful fetch.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    allCustomerOrders = JsonSerializer.Deserialize<List<CustomerOrderModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    // Stringify the item details of order for usage in the content view.
                    foreach(CustomerOrderModel customerOrder in allCustomerOrders)
                    {
                        string stringifiedItems = string.Empty;
                        foreach(OrderItemModel orderItem in customerOrder.OrderItems)
                        {
                            stringifiedItems = orderItem.OrderItemName + "(" + orderItem.Quantity + ")" + " ";
                        }
                        customerOrder.StringifiedItems = stringifiedItems;
                    }
                    return allCustomerOrders;
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

        public async Task<string> CancelCustomerOrder(CustomerOrderModel customerOrder)
        {
            return "sucess";
        }
    }
}
