using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.Setup.Locations;

namespace EFC.AgGateway.Integration.Models.ShipNotice
{
    public class ShipNoticeRequest
    {
        public Location Location { get; set; }
        public Vendor Vendor { get; set; }
        public string SeedYear { get; set; }
    }
}
