using FlamingFork.Helper.Utilities;
using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FlamingFork.Repositories.ApiServices
{
    public class MenuItemFetchServiceRepository : IMenuItemFetchServiceRepository
    {
        private string _Address;
        private HttpClient _HttpClient;

        public MenuItemFetchServiceRepository()
        {
            _Address = "10.10.100.242:8080";
            _HttpClient = new HttpClient();
        }

        #region Menu Items Fetcher

        public async Task<List<MenuItemModel>> GetMenuItems()
        {
            AllMenuItemsModel? fetchedMenuItems = new AllMenuItemsModel();
            ApiResponseMessageModal? ErrorResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            try
            {
                // Fetches authentication token from secure storage.
                string token = await SecureStorageHandler.GetAuthenticationToken();
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri("http://" + _Address + "/menu/allMenuItems");
                var response = await _HttpClient.GetAsync(uri);

                // Tries to deserialize the response to List<MenuItemModel> in case of sucessful fetch.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    fetchedMenuItems = JsonSerializer.Deserialize<AllMenuItemsModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return fetchedMenuItems.AllMenuItems;
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
