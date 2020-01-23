using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.Setup.Locations;
using System;

namespace EFC.AgGateway.Integration.Models
{
    public class PriceSheetRequest
    {
        public string MessageId { get; set; }
        public Location Location { get; set; }
        public Vendor Vendor { get; set; }
        public string ZoneId { get; set; }
        public string CropCode { get; set; }
        public DateTime LastRequestDate { get; set; }
        public string SeedYear { get; set; }
    }
}
