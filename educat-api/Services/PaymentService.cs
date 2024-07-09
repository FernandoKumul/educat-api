using Domain.Utilities;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace educat_api.Services
{
    public class PaymentService
    {
        private readonly PayPalSettings _payPalSettings;
        private readonly HttpClient _httpClient;

        public PaymentService(IOptions<PayPalSettings> settings)
        {
            _payPalSettings = settings.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.Value.BaseUri)
            };
        }

        public async Task<string> GenerateAccessTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_payPalSettings.ClientId) || string.IsNullOrEmpty(_payPalSettings.ClientSecret))
                {
                    throw new Exception("MISSING_API_CREDENTIALS");
                }

                var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_payPalSettings.ClientId}:{_payPalSettings.ClientSecret}"));

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

                using HttpResponseMessage response = await _httpClient.PostAsync("v1/oauth2/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                }));

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content);


                if (data is null) 
                {
                    throw new Exception("Token no obtenido");
                }

                return data["access_token"].ToString();
            } catch (Exception)
            {
                throw;
            }

        }

    }
}
