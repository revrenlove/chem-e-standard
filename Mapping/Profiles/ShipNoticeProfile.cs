using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using EFC.AgGateway.Integration.Models.ShipNotice;
using EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract;
using EFC.Data.Merchant.Common;
using ShipNoticeRequest = EFC.AgGateway.Integration.Models.ShipNotice.ShipNoticeRequest;
using ShipNoticeResponse = EFC.AgGateway.Integration.Models.ShipNotice.ShipNoticeResponse;

namespace EFC.AgGateway.Integration.Mapping.Profiles
{
    public class ShipNoticeProfile : Profile
    {
        public ShipNoticeProfile()
        {
            MapShipNoticeRequest();
            MapShipNoticeResponseDetail();
            CreateMap<ShipNoticeListType, ShipNoticeResponse>().ConvertUsing(new ShipNoticeResponseConverter());
            CreateMap<ShipNoticeBodyType, Shipment>().ConvertUsing(new ShipmentConverter());
        }

        private void MapShipNoticeRequest()
        {
            CreateMap<ShipNoticeRequest, ShipNoticeListRequestType>().ConvertUsing(shipNoticeRequest =>
            {
                return new ShipNoticeListRequestType
                {
                    Header = new HeaderType5
                    {
                        partnerId = shipNoticeRequest.Location.AgrimineEBID,
                        partnerType = "AGIIS-EBID",
                        messageId = Guid.NewGuid().ToString()
                    },
                    ShipNoticeListRequest = new ShipNoticeListRequestType1
                    {
                        Header = new HeaderType1
                        {
                            ThisDocumentIdentifier = new ThisDocumentIdentifierType { DocumentIdentifier = Guid.NewGuid().ToString() },
                            ThisDocumentDateTime = new ThisDocumentDateTimeType
                            {
                                DateTime = new DateTimeType
                                {
                                    DateTimeQualifier = cidxListDateQualifier.On,
                                    Value = DateTime.UtcNow
                                }
                            },
                            From = new FromType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { shipNoticeRequest.Location.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = shipNoticeRequest.Location.AgrimineEBID
                                        }
                                    },
                                    ContactInformation = new ContactInformationType[]
                                    {
                                        new ContactInformationType
                                        {
                                            ContactName = new string[] { shipNoticeRequest.SeedYear },
                                            ContactDescription = new string[] { "SeedYear" }
                                        },
                                        new ContactInformationType
                                        {
                                            ContactName = new string[] { "WS-XML" },
                                            ContactDescription = new string[] { "DataSource" }
                                        },
                                        new ContactInformationType
                                        {
                                            ContactName = new string[] { "MerchantAg" },
                                            ContactDescription = new string[] { "SoftwareName" }
                                        },
                                        new ContactInformationType
                                        {
                                            ContactName = new string[] { Assembly.GetExecutingAssembly().GetName().Version.ToString() },
                                            ContactDescription = new string[] { "SoftwareVersion" }
                                        }
                                    }
                                }
                            },
                            To = new ToType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { shipNoticeRequest.Vendor.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = shipNoticeRequest.Vendor.GLN.Trim()
                                        }
                                    }
                                }
                            }
                        },
                        ShipNoticeListRequestBody = new ShipNoticeListRequestBodyType
                        {
                            ShipNoticeListRequestProperties = new ShipNoticeListRequestPropertiesType(),
                            ShipNoticeListRequestPartners = new ShipNoticeListRequestPartnersType
                            {
                                Seller = new SellerType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { shipNoticeRequest.Vendor.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                                Value = shipNoticeRequest.Vendor.GLN.Trim()
                                            }
                                        }
                                    }
                                }
                            },
                            ShipNoticeListRequestDetails = new ShipNoticeListRequestDetailsType()
                        }
                    }
                };
            });
        }

        private class ShipNoticeResponseConverter : ITypeConverter<ShipNoticeListType, ShipNoticeResponse>
        {
            public ShipNoticeResponse Convert(ShipNoticeListType shipNoticeList, ShipNoticeResponse shipNoticeResponse, ResolutionContext context)
            {
                var details = shipNoticeList.ShipNoticeList.ShipNoticeListBody.ShipNoticeListDetails;

                shipNoticeResponse = new ShipNoticeResponse
                {
                    Shipments = context.Mapper.Map<IEnumerable<Shipment>>(details)
                };

                return shipNoticeResponse;
            }
        }

        private class ShipmentConverter : ITypeConverter<ShipNoticeBodyType, Shipment>
        {
            public Shipment Convert(ShipNoticeBodyType shipNoticeBody, Shipment shipment, ResolutionContext context)
            {
                shipment = new Shipment
                {
                    ShipmentNumber = shipNoticeBody.GetShipmentNumber(),
                    ShipDate = shipNoticeBody.GetShipDate(),
                    ProcessedTime = Session.GetServerTimeStamp(),
                    BillOfLadingNumber = shipNoticeBody.GetBillOfLadingNumber(),
                    ShipNoticeResponseDetails = context.Mapper.Map<IEnumerable<ShipNoticeResponseDetail>>(shipNoticeBody.ShipNoticeDetails.ShipNoticeProductLineItem)
                };

                shipment.ApplyPurchaseOrderNumber(shipNoticeBody.GetPurchaseOrderNumber());

                return shipment;
            }
        }

        private void MapShipNoticeResponseDetail()
        {
            CreateMap<ShipNoticeProductLineItemType, ShipNoticeResponseDetail>().ConvertUsing(shipNoticeProductLineItem =>
            {
                var shipNoticeResponseDetail = new ShipNoticeResponseDetail
                {
                    ProductIdentifier = shipNoticeProductLineItem.GetProductIdentifierInfo().Value,
                    ProductIdentifierAgency = shipNoticeProductLineItem.GetProductIdentifierInfo().Agency,
                    SalesOrderNumber = shipNoticeProductLineItem.GetSalesOrderNumber(),
                    ESequence = Convert.ToInt32(shipNoticeProductLineItem.LineNumber),
                    GTIN = shipNoticeProductLineItem.GetGTIN(),
                    VendorItemId = shipNoticeProductLineItem.GetVendorItemId(),
                    ItemDescription = shipNoticeProductLineItem.GetItemDescription(),
                    UOM = shipNoticeProductLineItem.GetUOM(),
                    QuantityOrdered = shipNoticeProductLineItem.GetQuantity(),
                    QuantityCommit = shipNoticeProductLineItem.GetQuantity(),
                    LotNumber = shipNoticeProductLineItem.GetLotNumber()
                };

                return shipNoticeResponseDetail;
            });
        }
    }
}
