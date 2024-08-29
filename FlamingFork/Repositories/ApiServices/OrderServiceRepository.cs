using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Net.Http.Headers;
using System.Text;
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
            _Address = "10.10.100.56:8080";
        }

        public async Task<List<CustomerOrderModel>> GetCustomerOrders()
        {
            CustomerOrderResponseModel? customerOrderResponse = new();
            ApiResponseMessageModal? errorResponse = new ();

            CustomerModel currentCustomer = await SecureStorageHandler.GetCustomerDetails();
            int customerId = Convert.ToInt16(currentCustomer.CustomerID);

            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/order/customerOrders/" + customerId);
                var response = await _HttpClient.GetAsync(uri);

                // Tries to deserialize the response to List<CustomerOrderModel> in case of sucessful fetch.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    customerOrderResponse = JsonSerializer.Deserialize<CustomerOrderResponseModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    // Stringify the item details of order for usage in the content view.
                    foreach(CustomerOrderModel customerOrder in customerOrderResponse.CustomerOrders)
                    {
                        string stringifiedItems = string.Empty;
                        int totalPrice = 0;
                        foreach(OrderItemModel orderItem in customerOrder.OrderItems)
                        {
                            totalPrice += orderItem.OrderItemPrice * orderItem.Quantity;
                            stringifiedItems += orderItem.OrderItemName + "(" + orderItem.Quantity + ")" + " ";
                        }
                        customerOrder.TotalPrice = totalPrice;
                        customerOrder.StringifiedItems = stringifiedItems;
                    }
                    return customerOrderResponse.CustomerOrders;
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
            ApiResponseMessageModal? cancelRequestResponse = new ApiResponseMessageModal();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Fetches authentication token from secure storage.
            string token = await SecureStorageHandler.GetAuthenticationToken();
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (customerOrder.OrderStatus == "Placed")
            {
                customerOrder.OrderStatus = "Cancelled";
                var jsonContent = JsonSerializer.Serialize<CustomerOrderModel>(customerOrder, options);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                // Tries to communicate with API and return suitable message.
                try
                {
                    var uri = new Uri("http://" + _Address + "/order/changeOrderStatus");
                    var response = await _HttpClient.PutAsync(uri,content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    cancelRequestResponse = JsonSerializer.Deserialize<ApiResponseMessageModal>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return cancelRequestResponse.Message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return $"The order is already {customerOrder.OrderStatus} and cannot be cancelled!";
            }
        }
    }
}
