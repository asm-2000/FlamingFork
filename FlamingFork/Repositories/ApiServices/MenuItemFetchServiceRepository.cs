using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Text.Json;

namespace FlamingFork.Repositories.ApiServices
{
    public class MenuItemFetchServiceRepository : IMenuItemFetchServiceRepository
    {
        private string _Address;
        private HttpClient _HttpClient;

        public MenuItemFetchServiceRepository()
        {
            _Address = "10.10.100.111:8080";
            _HttpClient = new HttpClient();
        }

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
