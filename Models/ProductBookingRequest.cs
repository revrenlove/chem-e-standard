using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.AccountsReceivable;
using EFC.Data.Merchant.DTOs.AccountsReceivable.Bookings;
using EFC.Data.Merchant.DTOs.Inventory;
using EFC.Data.Merchant.DTOs.Setup.Locations;
using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;
using EFC.Merchant.Enumerations.Orders;

namespace EFC.AgGateway.Integration.Models
{
    public class ProductBookingRequest
    {
        public Booking Booking { get; set; }
        public Vendor Vendor { get; set; }
        public Customer Customer { get; set; }
        public Item Item { get; set; }
        public Location Location { get; set; }
        public Payer Payer { get; set; } 
        public string SeedYear { get; set; }
        public ProductBookingType BookingType { get; set; }
        public string DocumentIdentifier { get; set; }
        public string LineNumber { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal QuantityChangedAmount { get; set; }
        public string UNRec20UnitOfMeasure { get; set; }
        public ProductBookingCustomerIdentifier CustomerIdentifier { get; set; }
    }   
}
