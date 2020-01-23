using System;
using System.Linq;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract
{
    public partial class ShipNoticeBodyType
    {
        public string GetBillOfLadingNumber()
        {
            return
                ShipNoticeProperties
                    .ReferenceInformation
                    .Single(referenceInformation =>
                    {
                        return referenceInformation.ReferenceType == cidxListReferenceType.BillOfLadingNumber;
                    })
                    .DocumentReference
                    .DocumentIdentifier;
        }

        public string GetPurchaseOrderNumber()
        {
            return
                ShipNoticeProperties
                    .PurchaseOrderInformation
                    .DocumentReference
                    .DocumentIdentifier;
        }

        public string GetShipmentNumber()
        {
            return 
                ShipNoticeProperties
                    .ShipmentIdentification?
                    .DocumentReference?
                    .DocumentIdentifier ?? string.Empty;
        }

        public DateTime GetShipDate()
        {
            return
                ShipNoticeProperties
                    .ShipDate
                    .DateTime
                    .Value;
        }
    }
}
