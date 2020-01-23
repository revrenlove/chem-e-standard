using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;
using System.Linq;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract
{
    public partial class ShipNoticeProductLineItemType
    {
        public string GetSalesOrderNumber()
        {
            return
                ReferenceInformation
                    .FirstOrDefault(info => info.ReferenceType == cidxListReferenceType.SalesOrderReference)?
                    .DocumentReference?
                    .DocumentIdentifier ?? string.Empty;
        }

        public string GetGTIN()
        {
            return
                GetProductIdentification(cidxListProductIDAgency.AGIISProductID)?.ProductIdentifier?.Value ??
                GetProductIdentification(cidxListProductIDAgency.GTIN)?.ProductIdentifier?.Value ??
                string.Empty;
        }

        public string GetVendorItemId()
        {
            return
                GetProductIdentification(cidxListProductIDAgency.AssignedBySeller)?.ProductIdentifier?.Value ??
                GetProductIdentification(cidxListProductIDAgency.EAN)?.ProductIdentifier?.Value ??
                string.Empty;
        }

        public string GetItemDescription()
        {
            return GetProductIdentification(cidxListProductIDAgency.AssignedBySeller)?.ProductName ??
                   GetProductIdentification(cidxListProductIDAgency.GTIN)?.ProductName ??
                   GetProductIdentification(cidxListProductIDAgency.EAN)?.ProductName ??
                   GetProductIdentification(cidxListProductIDAgency.AGIISProductID)?.ProductName ??
                   GetProductIdentification(cidxListProductIDAgency.UPC)?.ProductName ??
                   string.Empty;
        }

        public string GetLotNumber()
        {
            return ProductSubLineItems?[0]?.ManufacturingIdentificationDetails?.ManufacturingIdentificationNumber ?? string.Empty;
        }

        public string GetUOM()
        {
            return
                ShippedQuantity?
                    .Measurement?
                    .UnitOfMeasureCode?
                    .Value ?? string.Empty;
        }

        public decimal GetQuantity()
        {
            var measurementValue = InvoiceQuantity?.Measurement?.MeasurementValue;

            if (measurementValue == null)
            {
                measurementValue = 0;
            }

            return (decimal)(measurementValue.Value);
        }

        public (string Value, ShipNoticeProductIdentifierAgency Agency) GetProductIdentifierInfo()
        {
            var assignedBySellerValue = string.Empty;
            var eanValue = string.Empty;
            var agiisValue = string.Empty;
            var upcValue = string.Empty;

            foreach (var productIdentification in ProductIdentification)
            {
                if (productIdentification.ProductIdentifier.Agency.Equals(cidxListProductIDAgency.AssignedBySeller))
                {
                    assignedBySellerValue = productIdentification.ProductIdentifier.Value;
                }

                else if (productIdentification.ProductIdentifier.Agency.Equals(cidxListProductIDAgency.EAN))
                {
                    eanValue = productIdentification.ProductIdentifier.Value;
                }

                else if (productIdentification.ProductIdentifier.Agency.Equals(cidxListProductIDAgency.AGIISProductID))
                {
                    agiisValue = productIdentification.ProductIdentifier.Value;
                }

                else if (productIdentification.ProductIdentifier.Agency.Equals(cidxListProductIDAgency.UPC))
                {
                    upcValue = productIdentification.ProductIdentifier.Value;
                }
            }

            if (!string.IsNullOrWhiteSpace(assignedBySellerValue))
            {
                return (assignedBySellerValue, ShipNoticeProductIdentifierAgency.AssignedBySeller);
            }

            else if (!string.IsNullOrWhiteSpace(eanValue))
            {
                return (eanValue, ShipNoticeProductIdentifierAgency.EAN);
            }

            else if (!string.IsNullOrWhiteSpace(agiisValue))
            {
                return (agiisValue, ShipNoticeProductIdentifierAgency.AGIISProductID);
            }

            else if (!string.IsNullOrWhiteSpace(upcValue))
            {
                return (upcValue, ShipNoticeProductIdentifierAgency.UPC);
            }

            return (null, null);
        }

        private ProductIdentificationType GetProductIdentification(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .FirstOrDefault(identification =>
                    {
                        return identification.ProductIdentifier.Agency == agency;
                    });
        }
    }
}
