using FlamingFork.Models;
using FlamingFork.Repositories.Interfaces;
using System.Text.Json;
using System.Text;

namespace FlamingFork.Repositories.ApiServices
{
    public class AuthenticationServiceRepository : IAuthenticationServiceRepository
    {
        private HttpClient _HttpClient;

        public AuthenticationServiceRepository()
        {
            _HttpClient = new HttpClient();
        }

        #region SignUp Handler API Service

        public async Task<string> RegisterCustomer(CustomerModel customerDetails)
        {
            ApiResponseMessageModal? registerResponse = new();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize(customerDetails, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                var response = await _HttpClient.PostAsync("https://localHost:8080/user/registerCustomer", content);
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

            var jsonContent = JsonSerializer.Serialize(customerCredentials, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Commmnicates with API and returns received message.
            try
            {
                var response = await _HttpClient.PostAsync("https://localHost:8080/user/loginCustomer", content);

                // Tries to deserialize the response to LoginResponseModel in case of sucessful login.
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonSerializer.Deserialize<LoginResponseModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return "Sign In successful!";
                }
                // Deserializes the response to ApiResponseMessageModel in case of error satatus.
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
    }
}
