using System;
using System.Collections.Generic;

namespace EFC.AgGateway.Integration.Models.ShipNotice
{
    public class Shipment
    {
        public string VendorId { get; set; } = string.Empty;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string ShipmentNumber { get; set; } = string.Empty;
        public DateTime ShipDate { get; set; }
        public DateTime ProcessedTime { get; set; }
        public string BillOfLadingNumber { get; set; }
        public IEnumerable<ShipNoticeResponseDetail> ShipNoticeResponseDetails { get; set; } = new List<ShipNoticeResponseDetail>();

        public void ApplyPurchaseOrderNumber(string purchaseOrderNumber)
        {
            PurchaseOrderNumber = purchaseOrderNumber;

            foreach (var shipNoticeResponseDetail in ShipNoticeResponseDetails)
            {
                shipNoticeResponseDetail.PurchaseOrderNumber = purchaseOrderNumber;
            }
        }
    }
}
