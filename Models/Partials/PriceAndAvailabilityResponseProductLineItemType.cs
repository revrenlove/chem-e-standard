using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract
{
    public partial class PriceAndAvailabilityResponseProductLineItemType
    {
        public string GetIdByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentifier.Agency == agency;
                    })
                    .ProductIdentifier
                    .Value;
        }

        public string GetDescriptionByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentifier.Agency == agency;
                    })
                    .ProductDescription;                
        }

        public string GetCropCodeByByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentifier.Agency == agency;
                    })                    
                    .ProductClassification;
        }

        public string GetTraitByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentifier.Agency == agency;
                    })
                    .Trademark;
        }

        public string GetBrandByAgency(cidxListProductIDAgency agency)
        {
            return
                ProductIdentification
                    .Single(productInformation =>
                    {
                        return productInformation.ProductIdentifier.Agency == agency;
                    })
                    .ProductGradeDescription;
        }

        internal string GetProductCharacteristicsAgency(cidxListProductIDAgency agency)
        {
            return "";
        }
        public string GetUnitOfMeasure()
        {
            return (this.Item as ProductQuantityType).Measurement.UnitOfMeasureCode.Value;
        }
        public double GetMeasurementValue()
        {
            return (this.Item as ProductQuantityType).Measurement.MeasurementValue;
        }
    }
}
