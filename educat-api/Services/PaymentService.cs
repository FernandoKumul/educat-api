using Domain.DTOs.PayPal;
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

                var content = await response.Content.ReadFromJsonAsync<GetTokenInDTO>();

                if (content == null)
                {
                    throw new Exception("Información del toke no obtenido");
                }

                return content.Access_token;
            } catch (Exception)
            {
                throw;
            }

        }

        public async Task<object> CreateOrderAsync()
        {
            try
            {
                //Obtener información del carrito del id del usuario

                var accessToken = await GenerateAccessTokenAsync();
                var url = $"{_payPalSettings.BaseUri}/v2/checkout/orders";
                var payload = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                new
                {
                    amount = new
                    {
                        currency_code = "MXN",
                        value = "100.00"
                    }
                }
                }
                };
                var jsonPayload = JsonSerializer.Serialize(payload);


                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers =
                    {
                        { "Authorization", $"Bearer {accessToken}" }
                    },
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                };

                using var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

                if (data is null)
                {
                    throw new Exception("Data no obtenida");
                }


                return data;
            } catch (Exception)
            {
                throw;
            }

        }

    }
}
