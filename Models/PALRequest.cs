using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.Setup.Locations;
using System;

namespace EFC.AgGateway.Integration.Models
{
    public class PALRequest
    {
        public string MessageId { get; set; }
        public string SeedYear { get; set; }
        public DateTime ScheduleDateTime { get; set; }
        public Location Location { get; set; }
        public Vendor Vendor { get; set; }
        public Payer Payer { get; set; }
        public PALRequestLineItem[] PALRequestLineItems { get; set; }
    }
}
