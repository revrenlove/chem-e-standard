using EFC.AgGateway.Integration.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract
{
    public partial class ProductBookingResponseType
    {
        public string GetSalesOrderReferenceNumber()
        {
            return
                ProductBookingResponseBody[0]
                .ProductBookingResponseProperties
                .ReferenceInformation.SingleOrDefault(referenceInformation =>
                                      {
                                          return referenceInformation.ReferenceType == cidxListReferenceType.SalesOrderReference;
                                      })?
                .DocumentReference?
                .DocumentIdentifier ?? string.Empty;
        }

        public string GetLineItemNumber()
        {
            return
                ProductBookingResponseBody[0]
                .ProductBookingResponseDetails[0]
                .LineNumber.ToString("000000");
        }

        public string GetBookingId()
        {
            return
                ProductBookingResponseBody[0]
                .ProductBookingResponseProperties
                .ProductBookingOrderNumber;
        }

        public string GetDocumentIdentifier()
        {
            return
                Header
                .ThisDocumentIdentifier
                .DocumentIdentifier;
        }

        public string GetProductIdentifier()
        {
            return
                ProductBookingResponseBody[0]
                .ProductBookingResponseDetails[0]
                .ProductIdentification.SingleOrDefault(productIdentification =>
                                       {
                                           return productIdentification.ProductIdentifier.Agency == cidxListProductIDAgency.AGIISProductID;
                                       })?
                .ProductIdentifier?
                .Value ?? string.Empty;
        }

        public double GetQuantity()
        {
            return
                ProductBookingResponseBody[0]
                .ProductBookingResponseDetails[0]
                .ProductQuantity
                .Measurement
                .MeasurementValue;
        }

        public DateTime GetTimeStamp()
        {
            return
                Header
                .ThisDocumentDateTime
                .DateTime
                .Value;
        }

        public List<ProductBookingErrorMessage> GetPropertiesResponseDescriptons()
        {
            var errorMessages = new List<ProductBookingErrorMessage>();

            if (ProductBookingResponseBody[0].ProductBookingResponseProperties.ResponseStatus != null)
            {
                Array.ForEach(ProductBookingResponseBody[0].ProductBookingResponseProperties.ResponseStatus,
                              status => errorMessages.Add(new ProductBookingErrorMessage
                              {
                                  ReasonIdentifier = status.ResponseStatusReasonIdentifier.Value,
                                  ReasonDescription = status.ResponseStatusReasonDescription
                              }));
            }

            return errorMessages;
        }

        public List<ProductBookingErrorMessage> GetDetailsResponseDescriptions()
        {
            var errorMessages = new List<ProductBookingErrorMessage>();

            if (ProductBookingResponseBody[0].ProductBookingResponseDetails[0].ResponseStatus != null)
            {
                Array.ForEach(ProductBookingResponseBody[0].ProductBookingResponseDetails[0].ResponseStatus,
                              status => errorMessages.Add(new ProductBookingErrorMessage
                              {
                                  ReasonIdentifier = status.ResponseStatusReasonIdentifier.Value,
                                  ReasonDescription = status.ResponseStatusReasonDescription
                              }));
            }

            return errorMessages;
        }
    }
}
