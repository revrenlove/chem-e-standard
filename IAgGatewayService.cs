using System.Collections.Generic;
using EFC.AgGateway.Integration.Models;
using EFC.AgGateway.Integration.Models.ShipNotice;
using EFC.Data.Merchant.DTOs.Inventory;

namespace EFC.AgGateway.Integration
{
    /// <summary>
    /// Interface to be implemented by all AgGateway services
    /// </summary>
    public interface IAgGatewayService
    {
        /// <summary>
        /// Based upon the `PriceSheetRequest` dto supplied, this will
        /// call the appropriate E-Fulfillment service and return a list of tuples.
        /// </summary>
        /// <param name="priceSheetRequest">Dto containing all pertinent information for the request.</param>
        /// <param name="endpoint">Optional field to supply a specific endpoint to be appended to the base url.</param>
        /// <returns>Collection of tuples that contain an ImportSeedItem Dto and a ImportSeedItemZone Dto</returns>
        IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ZonePricingDetail)> GetSeedItemZonePricePairs(PriceSheetRequest priceSheetRequest, string endpoint = null);

        /// <summary>
        /// Based upon the `PALRequest` dto supplied, this will
        /// call the appropriate E-Fulfillment service and return a list of `SeedAvailabilityItem`s.
        /// </summary>
        /// <param name="palRequest">Dto containing all pertinent information for the request.</param>
        /// <param name="endpoint">Optional field to supply a specific endpoint to be appended to the base url.</param>
        /// <returns></returns>
        IEnumerable<SeedAvailabilityItem> GetPricingAndAvailability(PALRequest palRequest, string endpoint = null);

        ShipNoticeResponse GetShipNotices(ShipNoticeRequest shipNoticeRequest, string endpoint = null);

        ProductBookingResponse GetProductBookingResponse(ProductBookingRequest productBookingRequest, string endpoint = null);
    }
}
