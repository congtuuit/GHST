using Delivery.GHN.Models;

namespace Delivery.GHN
{
    // IGhnApiClient defines the contract for interacting with the GHN API.
    public interface IGhnApiClient
    {
        /// <summary>
        /// Asynchronously retrieves all shops based on the provided API configuration and request parameters.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="request">The request object containing parameters for fetching shops.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing all shops.</returns>
        Task<GhnApiResponse<GetAllShopsResponse>> GetAllShopsAsync(ApiConfig config, GetAllShopsRequest request);

        /// <summary>
        /// Asynchronously registers a new shop using the provided API configuration and request data.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="request">The request object containing parameters for registering a shop.</param>
        /// <returns>A task representing the asynchronous operation, with a response indicating the result of the registration.</returns>
        Task<GhnApiResponse<RegisterShopResponse>> RegisterShopAsync(ApiConfig config, RegisterShopRequest request);

        /// <summary>
        /// Asynchronously retrieves available services based on the provided API configuration and request parameters.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="request">The request object containing parameters for fetching available services.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing available services.</returns>
        Task<GhnApiResponse<List<AvailableServicesResponse>>> GetFeeServiceAsync(ApiConfig config, AvailableServicesRequest request);

        /// <summary>
        /// Asynchronously calculates the fee for a delivery based on the provided API configuration and request data.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="shopId">The ID of the shop for which to calculate the fee.</param>
        /// <param name="request">The request object containing parameters for fee calculation.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing the calculated fee.</returns>
        Task<GhnApiResponse<CalcFeeResponse>> GetFeeAsync(ApiConfig config, int shopId, CalcFeeRequest request);

        /// <summary>
        /// Asynchronously retrieves the SOC (Shipping Order Code) for a delivery order based on the provided API configuration and order code.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="shopId">The ID of the shop associated with the order.</param>
        /// <param name="orderCode">The order code for which to retrieve the SOC.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing the SOC details.</returns>
        Task<GhnApiResponse<FeeOrderSocResponse>> GetSocAsync(ApiConfig config, int shopId, string orderCode);

        // Create order methods

        /// <summary>
        /// Asynchronously creates a delivery order using the provided API configuration and request data.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="shopId">The ID of the shop creating the delivery order.</param>
        /// <param name="request">The request object containing parameters for creating the delivery order.</param>
        /// <returns>A task representing the asynchronous operation, with a response indicating the result of the order creation.</returns>
        Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDeliveryOrderAsync(ApiConfig config, int shopId, CreateDeliveryOrderRequest request);

        /// <summary>
        /// Asynchronously creates a draft delivery order using the provided API configuration and request data.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="shopId">The ID of the shop creating the draft delivery order.</param>
        /// <param name="request">The request object containing parameters for creating the draft delivery order.</param>
        /// <returns>A task representing the asynchronous operation, with a response indicating the result of the draft order creation.</returns>
        Task<GhnApiResponse<CreateDeliveryOrderResponse>> CreateDraftDeliveryOrderAsync(ApiConfig config, int shopId, CreateDeliveryOrderRequest request);

        // Order detail

        /// <summary>
        /// Asynchronously retrieves the details of a delivery order based on the provided API configuration and order code.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="orderCode">The order code for which to retrieve details.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing the order details.</returns>
        Task<GhnApiResponse<OrderDetailResponse>> DetailDeliveryOrderAsync(ApiConfig config, string orderCode);

        Task<GhnApiResponse<CancelOrderResponse>> CancelOrderAsync(ApiConfig config, List<string> orderCodes);

        // Location methods

        /// <summary>
        /// Asynchronously retrieves the list of provinces based on the provided API configuration.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing the list of provinces.</returns>
        Task<IEnumerable<ProvinceResponse>> GetProvinceAsync(ApiConfig config);

        /// <summary>
        /// Asynchronously retrieves the list of districts for a specified province based on the provided API configuration.
        /// </summary>
        /// <param name="config">The API configuration, including base URL and token.</param>
        /// <param name="provinceId">The ID of the province for which to retrieve districts.</param>
        /// <returns>A task representing the asynchronous operation, with a response containing the list of districts.</returns>
        Task<IEnumerable<DistrictResponse>> GetDistrictAsync(ApiConfig config, int provinceId);

        Task<IEnumerable<WardResponse>> GetWardAsync(ApiConfig config, int districtId);

        Task<IEnumerable<PickShiftResponse>> GetPickShiftAsync(ApiConfig config);
    }
}
