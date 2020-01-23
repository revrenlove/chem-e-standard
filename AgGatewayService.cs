using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EFC.AgGateway.Integration.Mapping;
using EFC.AgGateway.Integration.Models;
using EFC.AgGateway.Integration.Models.ShipNotice;
using EFC.AgGateway.Integration.Security;
using EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract;
using EFC.AgGateway.Integration.Utility;
using EFC.Data.Merchant.DTOs.Inventory;
using Generic = EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract;
using ShipNoticeRequest = EFC.AgGateway.Integration.Models.ShipNotice.ShipNoticeRequest;
using ShipNoticeResponse = EFC.AgGateway.Integration.Models.ShipNotice.ShipNoticeResponse;

namespace EFC.AgGateway.Integration
{
    /// <summary>
    /// The primary service to use to communicate with external AgGateway services
    /// </summary>
    public class AgGatewayService : IAgGatewayService
    {
        private readonly IAgGatewaySecurityProvider _agGatewaySecurityProvider;
        private readonly IMapper _mapper;

        private readonly string _baseUrl;

        /// <summary>
        /// Creates an instance of the AgGateway class
        /// </summary>
        /// <param name="agGatewaySecurityProvider">The security provider used to provide credentials to each request</param>
        /// <param name="baseUrl">The base url of the external web service</param>
        public AgGatewayService(
            IAgGatewaySecurityProvider agGatewaySecurityProvider,
            string baseUrl)
        {
            _agGatewaySecurityProvider = agGatewaySecurityProvider;

            _baseUrl = baseUrl;

            _mapper = AgGatewayMapping.GetMapper();
        }

        #region Exposed Methods

        /// <summary>
        /// Based upon the `PriceSheetRequest` dto supplied, this will
        /// call the appropriate E-Fulfillment service and return a list of tuples.
        /// </summary>
        /// <param name="priceSheetRequest">Dto containing all pertinent information for the request.</param>
        /// <param name="endpoint">Optional field to supply a specific endpoint to be appended to the base url.</param>
        /// <returns>Collection of tuples that contain an ImportSeedItem Dto and a ImportSeedItemZone Dto</returns>
        public IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ZonePricingDetail)> GetSeedItemZonePricePairs(PriceSheetRequest priceSheetRequest, string endpoint = null)
        {
            PriceSheetType priceSheet;

            var request = _mapper.Map<PriceSheetRequestType>(priceSheetRequest);

            var endpointAddress = ClientUtility.BuildEndpointAddress(_baseUrl, endpoint);
            var binding = ClientUtility.GetBinding(endpointAddress);

            using (var client = new CESOrderManagementPortTypeClient(binding, endpointAddress))
            {
                priceSheet = _agGatewaySecurityProvider.ExecuteRequest(client, request, client.PriceSheetRequest);
            }

            var seedItems = _mapper.Map<IEnumerable<(ImportSeedItem importSeedItem, ImportSeedItemZone zonePricingDetail)>>(priceSheet);

            return seedItems;
        }

        public IEnumerable<SeedAvailabilityItem> GetPricingAndAvailability(PALRequest palRequest, string endpoint = null)
        {
            var retItems = new List<SeedAvailabilityItem>();
            var request = _mapper.Map<Generic.PriceAndAvailabilityRequestType>(palRequest);


            var response = HandleGenericAgGatewayRequest<Generic.PriceAndAvailabilityRequestType, Generic.PriceAndAvailabilityResponseType>(request, endpoint);
            response.PriceAndAvailabilityResponseBody.PriceAndAvailabilityResponseDetails.ToList().ForEach(p =>
            {
                var mappedItem = _mapper.Map<SeedAvailabilityItem>(p);
                retItems.Add(mappedItem);
            });
            return retItems;
        }

        public ShipNoticeResponse GetShipNotices(ShipNoticeRequest shipNoticeRequest, string endpoint = null)
        {
            ShipNoticeListType shipNoticeListType;
            var request = _mapper.Map<ShipNoticeListRequestType>(shipNoticeRequest);

            var endpointAddress = ClientUtility.BuildEndpointAddress(_baseUrl, endpoint);
            var binding = ClientUtility.GetBinding(endpointAddress);

            using (var client = new CESTransportationPortTypeClient(binding, endpointAddress))
            {
                shipNoticeListType = _agGatewaySecurityProvider.ExecuteRequest(client, request, client.ShipNoticeListRequest);
            }

            var shipments = _mapper.Map<IEnumerable<Shipment>>(shipNoticeListType);

            foreach (var shipment in shipments)
            {
                shipment.VendorId = shipNoticeRequest.Vendor.Id;
            }

            return null;
        }

        public ProductBookingResponse GetProductBookingResponse(ProductBookingRequest productBookingRequest, string endpoint = null)
        {
            var request = _mapper.Map<Generic.ProductBookingType>(productBookingRequest);

            var response = HandleGenericAgGatewayRequest<Generic.ProductBookingType, Generic.ProductBookingResponseType>(request, endpoint);

            return _mapper.Map<ProductBookingResponse>(response);
        }

        // NOTE: ALL TYPES USED ARE STRICTLY HYPOTHETICAL
        // NOTE: THIS IS JUST AN ILLUSTRATION THAT WON'T BREAK THE BUILD
        /// <summary>
        /// This method takes an example request DTO, maps it to the appropriate AgGateway request object,
        ///     calls the HandleGenericAgGatewayRequest method to make the request and maps the response
        ///     to the appropriate DTO
        /// </summary>
        /// <param name="exampleRequest">The Request DTO</param>
        /// <param name="endpoint">The (optional) endpoint to be appended to the base url</param>
        /// <returns>The mapped DTO</returns>
        //public ExampleResponseDto ExampleGenericAgGatewayMethod(ExampleRequestDto exampleRequest, string endpoint = null)
        //{
        //    var request = _mapper.Map<Generic.ExampleAgGatewayRequestType>(exampleRequest);

        //    var response = HandleGenericAgGatewayRequest<Generic.ExampleAgGatewayRequestType, Generic.ExampleAgGatewayResponseType>(request, endpoint);

        //    return _mapper.Map<ExampleResponseDto>(response);
        //}

        #endregion

        /// <summary>
        /// This handles a request to the generic AgGateway Doc Exchange web service
        /// </summary>
        /// <typeparam name="TRequest">The request type from the `EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract` namespace</typeparam>
        /// <typeparam name="TResponse">The response type from the `EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract` namespace</typeparam>
        /// <param name="requestBody">The request object</param>
        /// <param name="endpoint">The (optional) endpoint to append to the base url</param>
        /// <returns></returns>
        private TResponse HandleGenericAgGatewayRequest<TRequest, TResponse>(TRequest requestBody, string endpoint = null)
             where TRequest : class
             where TResponse : class
        {
            var request = new Generic.inboundData
            {
                messageId = Guid.NewGuid().ToString(),
                xmlPayload = XmlUtility.Serialize(requestBody)
            };

            Generic.outboundData response;

            var endpointAddress = ClientUtility.BuildEndpointAddress(_baseUrl, endpoint);
            var binding = ClientUtility.GetBinding(endpointAddress);

            using (var client = new Generic.DocExchangePortTypeClient(binding, endpointAddress))
            {
                response = _agGatewaySecurityProvider.ExecuteRequest(client, request, client.execute);
            }

            return XmlUtility.Deserialize<TResponse>(response.xmlPayload[0]);
        }
    }
}
