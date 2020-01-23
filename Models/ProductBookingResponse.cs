using System;
using System.Collections.Generic;

namespace EFC.AgGateway.Integration.Models
{
    public class ProductBookingResponse
    {
        public string SalesOrderReferenceNumber { get; set; }
        public string LineItemNumber { get; set; }
        public string BookingId { get; set; }
        public string DocumentIdentifier { get; set; }
        public string ProductIdentifier { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<ProductBookingErrorMessage> PropertiesResponseDescriptions { get; set; }
        public List<ProductBookingErrorMessage> DetailsResponseDescriptions { get; set; }
    }
}
