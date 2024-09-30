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
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.GET_ALL_SHOPS, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<GetAllShopsResponse>>(jsonResponse);

                return result;
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while fetching shop information.");
                }

                throw;
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
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.REGISTER_SHOP, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<RegisterShopResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while registering shop.");
                }

                throw; // Rethrow the exception
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
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.GET_FEE_SERVICE, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<List<AvailableServicesResponse>>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while getting fee services.");
                }

                throw; // Rethrow the exception
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=76
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<CalcFeeResponse>> GetFeeAsync(ApiConfig config, int shopId, CalcFeeRequest request)
        {
            try
            {
                // Configure the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", $"{shopId}"); // Add ShopId header
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CALCULATE_FEE, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<CalcFeeResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while getting fee.");
                }

                throw; // Rethrow the exception
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=71
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<FeeOrderSocResponse>> GetSocAsync(ApiConfig config, int shopId, string orderCode)
        {
            try
            {
                // Configure the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", $"{shopId}"); // Add ShopId header
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonPayload = $@"{{ ""order_code"": ""{orderCode}"" }}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.FETCH_FEE_OF_ORDER, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<FeeOrderSocResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while getting SOC.");
                }
                throw; // Rethrow the exception
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
        public async Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDeliveryOrderAsync(ApiConfig config, int shopId, CreateDeliveryOrderRequest request)
        {
            try
            {
                // Configure the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", $"{shopId}"); // Add ShopId header
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CREATE_DELIVERY_ORDER, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<CreateDeliveryOrderResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while creating delivery order.");
                }
                throw; // Rethrow the exception
            }
        }

        /// <summary>
        /// https://api.ghn.vn/home/docs/detail?id=81
        /// </summary>
        /// <param name="config"></param>
        /// <param name="shopId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDraftDeliveryOrderAsync(ApiConfig config, int shopId, CreateDeliveryOrderRequest request)
        {
            try
            {
                // Configure the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Add("ShopId", $"{shopId}"); // Add ShopId header
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CREATE_DRAFT_ORDER, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<CreateDeliveryOrderResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while creating delivery order.");
                }
                throw; // Rethrow the exception
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
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonPayload = $@"{{ ""order_code"": ""{orderCode}"" }}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.ORDER_DETAIL, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<OrderDetailResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while get order detail.");
                }
                throw; // Rethrow the exception
            }
        }

        public async Task<GhnApiResponse<CancelOrderResponse>> CancelOrderAsync(ApiConfig config, List<string> orderCodes)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = "{\"order_codes\":[" + string.Join(",", orderCodes.Select(s => "\"" + s + "\"")) + "]}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiEndpoints.CANCEL_SHIFT, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GhnApiResponse<CancelOrderResponse>>(jsonResponse);
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while get order detail.");
                }
                throw; // Rethrow the exception
            }
        }

        #endregion

        public async Task<IEnumerable<ProvinceResponse>> GetProvinceAsync(ApiConfig config)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync(ApiEndpoints.GET_PROVINCE);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<ProvinceResponse>>>(jsonResponse);

                if (result != null && result.Code == 200)
                {
                    return result.Data;
                }

                return new List<ProvinceResponse>();
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while fetching shop information.");
                }

                return new List<ProvinceResponse>();
            }
        }

        public async Task<IEnumerable<DistrictResponse>> GetDistrictAsync(ApiConfig config, int provinceId)
        {
            try
            {
                // Set the base address for the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Construct the URL with the province ID as a query parameter
                var requestUrl = $"{ApiEndpoints.GET_DISTRICT}?province_id={provinceId}";

                // Send the GET request
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); // Throws an exception if the response status code is not successful

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into GhnApiResponse
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<DistrictResponse>>>(jsonResponse);
                if (result != null && result.Code == 200)
                {
                    return result.Data;
                }

                return new List<DistrictResponse>();
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while fetching district information.");
                }

                return new List<DistrictResponse>();
            }
        }

        public async Task<IEnumerable<WardResponse>> GetWardAsync(ApiConfig config, int districtId)
        {
            try
            {
                // Set the base address for the HttpClient
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Construct the URL with the province ID as a query parameter
                var requestUrl = $"{ApiEndpoints.GET_WARD}?district_id={districtId}";

                // Send the GET request
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode(); // Throws an exception if the response status code is not successful

                // Read the response content
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into GhnApiResponse
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<WardResponse>>>(jsonResponse);
                if (result != null && result.Code == 200)
                {
                    return result.Data;
                }

                return new List<WardResponse>();
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while fetching district information.");
                }

                return new List<WardResponse>();
            }
        }

        public async Task<IEnumerable<PickShiftResponse>> GetPickShiftAsync(ApiConfig config)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(config.BaseUrl);
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync(ApiEndpoints.PICK_SHIFT);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GhnApiResponse<IEnumerable<PickShiftResponse>>>(jsonResponse);

                if (result != null && result.Code == 200)
                {
                    return result.Data;
                }

                return new List<PickShiftResponse>();
            }
            catch (Exception ex)
            {
                if (_allowLogging)
                {
                    Log.Error(ex, "Error occurred while fetching shop information.");
                }

                return new List<PickShiftResponse>();
            }
        }
    }
}

