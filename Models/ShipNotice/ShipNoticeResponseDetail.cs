using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;

namespace EFC.AgGateway.Integration.Models.ShipNotice
{
    public class ShipNoticeResponseDetail
    {
        public string ProductIdentifier { get; set; }
        public ShipNoticeProductIdentifierAgency ProductIdentifierAgency { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string SalesOrderNumber { get; set; }
        public int ESequence { get; set; }
        public string GTIN { get; set; }
        public string VendorItemId { get; set; }
        public string ItemDescription { get; set; }
        public string UOM { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityCommit { get; set; }
        public string LotNumber { get; set; }
    }
}
