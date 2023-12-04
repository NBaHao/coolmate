using CoolMate.Models;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using CoolMate.Models.Payment;

namespace CoolMate.Services
{
    public class MomoPayment
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        public async Task<string> CreatePaymentAsync(ShopOrder order)
        {
            string accessKey = _configuration.GetSection("Momo:AccessKey").Value;
            string secretKey = _configuration.GetSection("Momo:SecretKey").Value;

            MomoQuickPayResquest request = new MomoQuickPayResquest();
            request.orderInfo = "Thanh toán qua ví MoMo";
            request.partnerCode = _configuration.GetSection("Momo:PartnerCode").Value;
            request.redirectUrl = _configuration.GetSection("Momo:ReturnUrl").Value;
            request.ipnUrl = _configuration.GetSection("Momo:NotifyUrl").Value; 
            request.amount = (long)order.OrderTotal;
            request.orderId = order.Id.ToString() + ":" + DateTime.Now.Ticks.ToString();
            request.extraData = "";
            request.lang = "vi";
            request.requestId = DateTime.Now.Ticks.ToString();
            request.requestType = "captureWallet";

            var rawSignature = "accessKey=" + accessKey +
                "&amount=" + request.amount +
                "&extraData=" + request.extraData +
                "&ipnUrl=" + request.ipnUrl +
                "&orderId=" + request.orderId +
                "&orderInfo=" + request.orderInfo +
                "&partnerCode=" + request.partnerCode +
                "&redirectUrl=" + request.redirectUrl +
                "&requestId=" + request.requestId +
                "&requestType=" + request.requestType;

            request.signature = Helpers.HashHelper.HmacSHA256(rawSignature, secretKey);

            var requestData = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8,
                "application/json");

            var quickPayResponse = await _client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", requestContent);
            var contents = quickPayResponse.Content.ReadAsStringAsync().Result;
            return contents;
        }
    }
}
