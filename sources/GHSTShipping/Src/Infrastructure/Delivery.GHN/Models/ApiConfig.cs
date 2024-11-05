namespace Delivery.GHN.Models
{
    public class ApiConfig
    {
        private string _baseUrl;
        private string _token;
        private string _shopId;

        public ApiConfig(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            _token = token;
        }

        public ApiConfig(string baseUrl, string token, string shopId)
        {
            _baseUrl = baseUrl;
            _token = token;
            _shopId = shopId;
        }

        public string BaseUrl { get { return _baseUrl; } }
        public string Token { get { return _token; } }
        public string ShopId { get { return _shopId; } }
    }
}
