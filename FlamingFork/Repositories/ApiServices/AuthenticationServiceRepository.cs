using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Text.Json;
using System.Text;
using FlamingFork.Helper.Utilities;

namespace FlamingFork.Repositories.ApiServices
{
    public class AuthenticationServiceRepository : IAuthenticationServiceRepository
    {
        private string _Address;
        private HttpClient _HttpClient;

        public AuthenticationServiceRepository()
        {
            _HttpClient = new HttpClient();
            _Address = "192.168.10.75:8080";
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

        #region Details Update Handler API Service
        public async Task<string> UpdateCustomerDetails(CustomerModel updatedDetails)
        {
            CustomerDetailsUpdateResponseModel? updateResponse = new CustomerDetailsUpdateResponseModel();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize<CustomerModel>(updatedDetails, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                var uri = new Uri("http://" + _Address + "/user/updateCustomerDetails");
                var response = await _HttpClient.PutAsync(uri, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserializes response into CustomerDetailsUpdateResponseModel.
                updateResponse = JsonSerializer.Deserialize<CustomerDetailsUpdateResponseModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response.IsSuccessStatusCode)
                {
                    string customerDetails = JsonSerializer.Serialize(updateResponse.CustomerDetails);
                    SecureStorage.Remove("CustomerDetails");
                    await SecureStorage.SetAsync("CustomerDetails", customerDetails);
                    return updateResponse.Message;
                }   
                else
                {
                    return updateResponse.Message;
                }
            }
            // Returns exception message if communication with API fails.
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
