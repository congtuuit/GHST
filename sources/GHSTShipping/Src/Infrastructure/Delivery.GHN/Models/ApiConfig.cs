namespace Delivery.GHN.Models
{
    public class ApiConfig
    {
        private string _baseUrl;
        private string _token;

        public ApiConfig(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            _token = token;
        }

        public string BaseUrl { get { return _baseUrl; } }
        public string Token { get { return _token; } }
    }
}
