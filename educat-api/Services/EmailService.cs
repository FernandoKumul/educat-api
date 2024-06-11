using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace educat_api.Services
{
    public class EmailService
    {
        private readonly string _apiKey = "5e0b68cfbb2c5534a846d81f4d5ece92";
        private readonly string _apiSecret = "58b098e691c007c8353836ba6d69b03c";

        public async Task SendVerificationEmail(string email, string token, string fullName)
        {
            try
            {
                MailjetClient client = new MailjetClient(_apiKey, _apiSecret);
                MailjetRequest request = new MailjetRequest
                {
                    //Resource = Send.Resource,
                    Resource = SendV31.Resource
                }
                .Property(Send.Messages, new JArray {
                new JObject {
                    {"From", new JObject {
                        {"Email", "fernando.kh2003@gmail.com"},
                        {"Name", "Educat" }
                    }},
                    {"To", new JArray {
                        new JObject {
                            {"Email", email},
                            {"Name", fullName}
                        }
                    }},
                    {"TemplateID", 6025295},
                    {"TemplateLanguage", true},
                    {"Variables", new JObject {
                        {"token", token}
                    }}
                }
                });
                MailjetResponse response = await client.PostAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: ${response.GetErrorMessage()}, ${response.GetErrorInfo()}");
                }
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
