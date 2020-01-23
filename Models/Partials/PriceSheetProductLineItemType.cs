using System;
using System.Linq;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract
{
    public partial class PriceSheetProductLineItemType
    {
        public string GetIdByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductInformation
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentification.ProductIdentifier.Agency == agency;
                    })
                    .ProductIdentification
                    .ProductIdentifier
                    .Value;
        }

        public string GetDescriptionByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductInformation
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentification.ProductIdentifier.Agency == agency;
                    })
                    .ProductIdentification
                    .ProductDescription;
        }

        public string GetCropCodeByByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductInformation
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentification.ProductIdentifier.Agency == agency;
                    })
                    .ProductIdentification
                    .ProductClassification;
        }

        public PricingPerUnitType GetPricingPerUnitByType(cidxListPriceType priceType)
        {
            var listPrice = PriceSheetPriceData[0].ListPrice;

            var pricingType = listPrice.Single(p => p.PriceType == priceType);

            var items = pricingType.Items;

            var filteredItems = items.Where(i => i.GetType() == typeof(PricingPerUnitType));

            var pricingPerUnits = filteredItems.Select(i => (PricingPerUnitType)i);

            var pricing = pricingPerUnits.Single();

            return pricing;
        }

        public string GetZone()
        {
            return 
                PriceSheetPriceData[0]
                    .PriceApplicabilityCriteria
                    .GeographicFeatures[0]
                    .Items
                    .Where(item => item.GetType() == typeof(string))
                    .Select(zoneObject => (string)zoneObject)
                    .Single();
        }

        public string GetProductAttributeByAgencyAndName(cidxListTestMethodAgency agency, string name)
        {
            return PriceSheetPriceData[0]
                .PriceApplicabilityCriteria
                .ProductFeatures
                .ProductAttribute
                    .SingleOrDefault(productAttribute =>
                    {
                        return productAttribute.ProductAttributeName.Agency == agency &&
                               string.Equals(productAttribute.ProductAttributeName.Value, name, StringComparison.OrdinalIgnoreCase);
                    })?.ProductAttributeValue ?? string.Empty;
        }

    }
}
