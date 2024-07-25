using Domain.DTOs.PayPal;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Context;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDBContext _context;
        private readonly CartWishService _cartWishService;

        public PaymentService(IOptions<PayPalSettings> settings, AppDBContext context, CartWishService cartWishService)
        {
            _payPalSettings = settings.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.Value.BaseUri)
            };
            _context = context;
            _cartWishService = cartWishService;

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

        public async Task<object> CreateOrderAsync(int userId)
        {
            try
            {
                //Obtener información del carrito del id del usuario

                var accessToken = await GenerateAccessTokenAsync();
                var url = $"{_payPalSettings.BaseUri}/v2/checkout/orders";

                var cartItems = await _cartWishService.GetCartItemsByUserId(userId);

                if(!cartItems.Any())
                {
                    throw new Exception("El carrito está vacío, no tienes algo que comprar");
                }

                decimal totalAmount = 0;

                foreach (var cartItem in cartItems)
                {
                    totalAmount += cartItem.Course.Price ?? 0;
                }

                var payload = new CreateOrderOutDTO
                {
                    purchase_units = new PurchaseUnits[]
                    {
                        new PurchaseUnits
                        {
                            amount = new AmountCreateOrder
                            {
                                value = totalAmount.ToString()
                            }
                        }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(payload);


                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers =
                    {
                        { "Authorization", $"Bearer {accessToken}" },
                        //{ "PayPal-Mock-Response", "{\"mock_application_codes\": \"INTERNAL_SERVER_ERROR\"}"}
                    },
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                };

                using var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();


                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseContent);

                if (data is null)
                {
                    throw new Exception("Data no obtenida");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new PayPalException("Error al crear la orden en PayPal", data);
                }

                return data;
            } catch (Exception)
            {
                throw;
            }

        }

        public async Task<CaptureOrderInDTO> CaptureOrder(string orderId)
        {
            try
            {
                var accessToken = await GenerateAccessTokenAsync();
                var url = $"{_payPalSettings.BaseUri}/v2/checkout/orders/{orderId}/capture";

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers =
                    {
                        { "Authorization", $"Bearer {accessToken}" },
                        //{ "PayPal-Mock-Response", "{\"mock_application_codes\": \"TRANSACTION_REFUSED\"}"}
                    },
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                };

                using var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();


                var data = JsonSerializer.Deserialize<JsonElement>(responseContent);


                if (!response.IsSuccessStatusCode)
                {
                    throw new PayPalException("Error al crear la orden en PayPal", data);
                }

                var orderJsonString = data.GetRawText();
                var order = JsonSerializer.Deserialize<CaptureOrderInDTO>(orderJsonString);

                return order;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task CreatePayments(int userId, CaptureOrderInDTO infoOrder)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var cartItems = await _context.CartWishList
                    .Include(c => c.Course)
                    .Where(c => c.Type == "cart" && c.FkUser == userId)
                    .ToListAsync();

                var newCartItemsList = new List<Payment>();

                var coursesId = new List<int>();

                foreach (var cartItem in cartItems)
                {
                    Payment newPayment = new()
                    {
                        FkCourse = cartItem.FkCourse,
                        FkUser = cartItem.FkUser,
                        OrderId = infoOrder.id,
                        PayerId = infoOrder.payer.payer_id,
                        PaymentAmount = cartItem.Course.Price ?? 0,
                        PayerEmail = infoOrder.payer.email_address,
                        PaymentMethod = "",
                        CardType = "",
                        PaymentStatus = infoOrder.status,
                        Currency = infoOrder.purchase_units[0].payments.captures[0].amount.currency_code,
                        TransactionDate = infoOrder.purchase_units[0].payments.captures[0].update_time,

                    };

                    newCartItemsList.Add(newPayment);
                    coursesId.Add(cartItem.FkCourse);
                }
                
                await _context.AddRangeAsync(newCartItemsList.AsEnumerable());
                _context.CartWishList.RemoveRange(cartItems);

                //Eliminar los cursos comprados de la lista de deseos
                await _context.CartWishList
                    .Where(c => c.FkUser == userId && coursesId.Contains(c.FkCourse) && c.Type == "wish")
                    .ExecuteDeleteAsync();

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        public class PayPalException : Exception
        {
            public object ErrorData { get; }

            public PayPalException(string message, object errorData) : base(message)
            {
                ErrorData = errorData;
            }
        }


    }
}
