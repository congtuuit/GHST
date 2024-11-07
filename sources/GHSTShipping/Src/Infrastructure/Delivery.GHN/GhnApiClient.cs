using Delivery.GHN.Constants;
using Delivery.GHN.Models;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace Delivery.GHN
{
    public class GhnApiClient : IGhnApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly bool _allowLogging = false;

        public GhnApiClient(bool allowLogging = false)
        {
            _httpClient = new HttpClient();
            _allowLogging = allowLogging;

            if (allowLogging)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs\\log_GhnApiClient-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=79
        /// </summary>
        /// <param name="config"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<GetAllShopsResponse>> GetAllShopsAsync(ApiConfig config, GetAllShopsRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("Token");
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.GET_ALL_SHOPS, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<GetAllShopsResponse>>(jsonResponse);

                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching shop information.");
                throw new Exception("Failed to fetch shop information due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching shop information.");
                throw new Exception("Failed to parse the shop information response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while fetching shop information.");
                throw; // Rethrow the original exception
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=58
        /// </summary>
        /// <param name="config"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<RegisterShopResponse>> RegisterShopAsync(ApiConfig config, RegisterShopRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("Token");
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.REGISTER_SHOP, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<RegisterShopResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while registering shop.");
                throw new Exception("Failed to register shop due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while registering shop.");
                throw new Exception("Failed to parse the shop registration response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while registering shop.");
                throw; // Rethrow the original exception
            }
        }


        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=77
        /// </summary>
        /// <param name="config"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<List<AvailableServicesResponse>>> GetFeeServiceAsync(ApiConfig config, AvailableServicesRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("Token");
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.GET_FEE_SERVICE, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<List<AvailableServicesResponse>>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while getting fee services.");
                throw new Exception("Failed to get fee services due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while getting fee services.");
                throw new Exception("Failed to parse the fee services response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while getting fee services.");
                throw; // Rethrow the original exception
            }
        }


        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=76
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<CalcFeeResponse>> GetFeeAsync(ApiConfig config, string shopId, CalcFeeRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("ShopId");
                _httpClient.DefaultRequestHeaders.Add("ShopId", shopId); // Add ShopId header

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CALCULATE_FEE, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<CalcFeeResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while calculating fee for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to calculate fee due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while calculating fee for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to parse the fee calculation response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while getting fee for ShopId: {ShopId}", shopId);
                throw; // Rethrow the original exception
            }
        }


        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=71
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<FeeOrderSocResponse>> GetSocAsync(ApiConfig config, string shopId, string orderCode)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("ShopId");
                _httpClient.DefaultRequestHeaders.Add("ShopId", shopId); // Add ShopId header

                var jsonPayload = $@"{{ ""order_code"": ""{orderCode}"" }}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.FETCH_FEE_OF_ORDER, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<FeeOrderSocResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching SOC for order code: {OrderCode}", orderCode);
                throw new Exception("Failed to fetch SOC due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching SOC for order code: {OrderCode}", orderCode);
                throw new Exception("Failed to parse the SOC response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while getting SOC for order code: {OrderCode}", orderCode);
                throw; // Rethrow the original exception
            }
        }


        #region Create order
        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=123
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDeliveryOrderAsync(ApiConfig config, string shopId, CreateDeliveryOrderRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("Token");
                _httpClient.DefaultRequestHeaders.Remove("ShopId");
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", shopId);

                // Serialize request content
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var apiResponse = await _httpClient.PostAsync(ApiEndpoints.CREATE_DELIVERY_ORDER, content);

                // Deserialize response content
                var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GhnApiResponse<CreateDeliveryOrderResponse>>(jsonResponse)
                               ?? throw new InvalidOperationException("Received a null response from the API.");

                return response;
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while creating delivery order for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to create delivery order due to an HTTP request error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to parse the delivery order response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Unexpected error occurred while creating delivery order for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to create delivery order.", ex);
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=81
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDraftDeliveryOrderAsync(ApiConfig config, string shopId, CreateDeliveryOrderRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Add dynamic headers
                _httpClient.DefaultRequestHeaders.Remove("ShopId");
                _httpClient.DefaultRequestHeaders.Add("ShopId", shopId); // Add ShopId header

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CREATE_DRAFT_ORDER, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<CreateDeliveryOrderResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while creating draft delivery order for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to create draft delivery order due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while creating draft delivery order for ShopId: {ShopId}", shopId);
                throw new Exception("Failed to parse draft delivery order response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while creating draft delivery order for ShopId: {ShopId}", shopId);
                throw; // Rethrow the original exception
            }
        }


        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=66
        /// </summary>
        /// <param name="config"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<OrderDetailResponse>> DetailDeliveryOrderAsync(ApiConfig config, string orderCode)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                var jsonPayload = $@"{{ ""order_code"": ""{orderCode}"" }}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.ORDER_DETAIL, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<OrderDetailResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while getting order detail for OrderCode: {OrderCode}", orderCode);
                throw new Exception("Failed to get order detail due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while getting order detail for OrderCode: {OrderCode}", orderCode);
                throw new Exception("Failed to parse order detail response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while getting order detail for OrderCode: {OrderCode}", orderCode);
                throw; // Rethrow the original exception
            }
        }


        public async Task<GhnApiResponse<CancelOrderResponse>> CancelOrderAsync(ApiConfig config, List<string> orderCodes)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                string jsonPayload = "{\"order_codes\":[" + string.Join(",", orderCodes.Select(s => "\"" + s + "\"")) + "]}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CANCEL_SHIFT, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<CancelOrderResponse>>(jsonResponse);

                // Return the result or throw if it's null
                return result ?? throw new InvalidOperationException("Received a null response from the API.");
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while canceling orders: {OrderCodes}", string.Join(", ", orderCodes));
                throw new Exception("Failed to cancel orders due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while canceling orders: {OrderCodes}", string.Join(", ", orderCodes));
                throw new Exception("Failed to parse cancel order response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while canceling orders: {OrderCodes}", string.Join(", ", orderCodes));
                throw; // Rethrow the original exception
            }
        }


        #endregion

        public async Task<IEnumerable<ProvinceResponse>> GetProvinceAsync(ApiConfig config)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                var response = await _httpClient.GetAsync(ApiEndpoints.GET_PROVINCE);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<ProvinceResponse>>>(jsonResponse);

                // Check if the result is valid and contains the expected code
                if (result != null && result.Code == 200)
                {
                    return result.Data;
                }

                return Enumerable.Empty<ProvinceResponse>();
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching provinces.");
                throw new Exception("Failed to fetch provinces due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching provinces.");
                throw new Exception("Failed to parse province response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while fetching provinces.");
                return Enumerable.Empty<ProvinceResponse>();
            }
        }


        public async Task<IEnumerable<DistrictResponse>> GetDistrictAsync(ApiConfig config)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Construct the URL for fetching districts
                var requestUrl = ApiEndpoints.GET_DISTRICT;

                // Send the GET request
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<DistrictResponse>>>(jsonResponse);

                // Validate the result and return data
                return result != null && result.Code == 200 ? result.Data : Enumerable.Empty<DistrictResponse>();
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching districts.");
                throw new Exception("Failed to fetch districts due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching districts.");
                throw new Exception("Failed to parse district response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while fetching district information.");
                return Enumerable.Empty<DistrictResponse>();
            }
        }


        public async Task<IEnumerable<WardResponse>> GetWardAsync(ApiConfig config, int districtId)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Construct the URL for fetching wards with the district ID as a query parameter
                var requestUrl = $"{ApiEndpoints.GET_WARD}?district_id={districtId}";

                // Send the GET request
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<WardResponse>>>(jsonResponse);

                // Validate the result and return data
                return result != null && result.Code == 200 ? result.Data : Enumerable.Empty<WardResponse>();
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching wards.");
                throw new Exception("Failed to fetch wards due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching wards.");
                throw new Exception("Failed to parse ward response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while fetching ward information.");
                return Enumerable.Empty<WardResponse>();
            }
        }


        public async Task<IEnumerable<PickShiftResponse>> GetPickShiftAsync(ApiConfig config)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                // Send the GET request to fetch pick shifts
                var response = await _httpClient.GetAsync(ApiEndpoints.PICK_SHIFT);
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<PickShiftResponse>>>(jsonResponse);

                // Validate the result and return data
                return result != null && result.Code == 200 ? result.Data : Enumerable.Empty<PickShiftResponse>();
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                Log.Error(httpEx, "HTTP request error while fetching pick shifts.");
                throw new Exception("Failed to fetch pick shifts due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                Log.Error(jsonEx, "JSON deserialization error while fetching pick shifts.");
                throw new Exception("Failed to parse pick shift response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                Log.Error(ex, "Error occurred while fetching pick shift information.");
                return Enumerable.Empty<PickShiftResponse>();
            }
        }

        public async Task<SearchOrderResponse> SearchOrdersAsync(
            ApiConfig config,
            ShippingOrderSearchRequest request)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                _httpClient.DefaultRequestHeaders.Add("referer", "https://khachhang.ghn.vn/");
                _httpClient.DefaultRequestHeaders.Add("shopid", config.ShopId);
                _httpClient.DefaultRequestHeaders.Add("x-request-id", Guid.NewGuid().ToString());

                var query = JsonConvert.SerializeObject(request);
                var content = new StringContent(query, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.SEARCH, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<SearchOrderResponse>>(jsonResponse);

                // Validate the result and return data
                return result != null && result.Code == 200 ? result.Data : new SearchOrderResponse();
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                throw new Exception("Failed to fetch search due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                throw new Exception("Failed to parse response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                return new SearchOrderResponse();
            }
        }

        public async Task<string> CountOrderByStatusAsync(ApiConfig config)
        {
            try
            {
                // Ensure HttpClient is configured
                ConfigureHttpClient(config);

                _httpClient.DefaultRequestHeaders.Add("referer", "https://khachhang.ghn.vn/");
                _httpClient.DefaultRequestHeaders.Add("shopid", config.ShopId);
                _httpClient.DefaultRequestHeaders.Add("x-request-id", Guid.NewGuid().ToString());

                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    shop_id = config.ShopId,
                    source = "5sao"
                }), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiEndpoints.COUNT_ORDER_BY_STATUS, content);

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Validate the result and return data
                return jsonResponse;
            }
            catch (HttpRequestException httpEx) when (_allowLogging)
            {
                throw new Exception("Failed to fetch search due to an HTTP error.", httpEx);
            }
            catch (JsonException jsonEx) when (_allowLogging)
            {
                throw new Exception("Failed to parse response.", jsonEx);
            }
            catch (Exception ex) when (_allowLogging)
            {
                return string.Empty;
            }
        }


        // Helper method for configuring the HttpClient
        private void ConfigureHttpClient(ApiConfig config)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
            }
            _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
            _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

