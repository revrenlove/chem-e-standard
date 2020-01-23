using System.Collections.Generic;
using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.Setup.Locations;

namespace EFC.AgGateway.Integration.Models.ShipNotice
{
    public class ShipNoticeResponse
    {
        public Vendor Vendor { get; set; }
        public Location Location { get; set; }
        public IEnumerable<Shipment> Shipments { get; set; }
    }
}
