using System;
using System.Reflection;
using AutoMapper;
using EFC.AgGateway.Integration.Models;
using EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract;
using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Data.Merchant.DTOs.Inventory;
using EFC.Data.Merchant.DTOs.Setup.Locations;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace EFC.AgGateway.Integration.Mapping.Profiles
{
    public class PALProfile : Profile
    {
        public PALProfile()
        {
            MapPriceAndAvailabilityItemToSeedAvailabilityItem();
            MapPriceAndAvailabilityRequest();
        }

        private void MapPriceAndAvailabilityItemToSeedAvailabilityItem()
        {
            CreateMap<PriceAndAvailabilityResponseProductLineItemType, SeedAvailabilityItem>().ConvertUsing(seedAvailabilityItem =>
            {
                string upc = seedAvailabilityItem.GetIdByAgency(cidxListProductIDAgency.UPC);
                string description = seedAvailabilityItem.GetDescriptionByAgency(cidxListProductIDAgency.UPC);
                string cropCode = seedAvailabilityItem.GetCropCodeByByAgency(cidxListProductIDAgency.UPC);
                string crop = seedAvailabilityItem.GetCropCodeByByAgency(cidxListProductIDAgency.UPC);
                string trait = seedAvailabilityItem.GetTraitByAgency(cidxListProductIDAgency.UPC);
                string brand = seedAvailabilityItem.GetBrandByAgency(cidxListProductIDAgency.UPC);
                string unitOfMesaure = seedAvailabilityItem.GetUnitOfMeasure();
                double measurementValue = seedAvailabilityItem.GetMeasurementValue();

                var retSeedAvailabilityItem = new SeedAvailabilityItem
                {
                    CropCodeDescription = description,
                    UPC = upc,
                    CropCode = cropCode,
                    Trait = trait,
                    Brand = brand,
                    RelativeMaturity = 1,
                    LineNumber = seedAvailabilityItem.LineNumber,
                    ScheduleDateTimeInformation = ((DateTimeType)seedAvailabilityItem.ScheduleDateTimeInformation.DateTimeInformation.Item).Value,
                    UnitOfMeasureCode = unitOfMesaure,
                    Crop = crop,
                    MeasurementValue = measurementValue.ToString()
                };

                return retSeedAvailabilityItem;
            });
        }
        private void MapPriceAndAvailabilityRequest()
        {
            CreateMap<PALRequest, PriceAndAvailabilityRequestType>().ConvertUsing(palRequest =>
            {
                var mappedRequestLineItems = new List<PriceAndAvailabilityRequestProductLineItemType>();

                palRequest.PALRequestLineItems.ToList().ForEach(p =>
                {
                    var mappedItem = new PriceAndAvailabilityRequestProductLineItemType
                    {
                        LineNumber = p.LineNumber,
                        RequisitionLineItemNumber = p.LineNumber.ToString(),
                        ProductIdentification = new ProductIdentificationType[]
                            {
                                new ProductIdentificationType
                                {
                                    ProductIdentifier = new ProductIdentifierType
                                    {
                                        Agency = cidxListProductIDAgency.AGIISProductID,
                                        Value = p.GTIN
                                    },
                                    ProductClassification = p.CropCode
                                },
                            },
                                ProductQuantity = new ProductQuantityType
                                {
                                    Measurement = new MeasurementType
                                    {
                                        MeasurementValue = p.QTY,
                                        UnitOfMeasureCode = new UnitOfMeasureCodeType
                                        {
                                            Domain = "UN-Rec-20",
                                            Value = p.PUOM
                                        }
                                    }
                                },
                                ScheduleDateTimeInformation = new ScheduleDateTimeInformationType[]
                            {
                                            new ScheduleDateTimeInformationType
                                            {
                                                DateTimeInformation = new DateTimeInformationType
                                                {
                                                    Item = new DateTimeType
                                                    {
                                                        DateTimeQualifier = cidxListDateQualifier.On,
                                                        Value = palRequest.ScheduleDateTime
                                                    }
                                                },
                                                ScheduleType = cidxListScheduleDateTimeType.RequestedDelivery
                                            }
                            }
                    };
                    mappedRequestLineItems.Add(mappedItem);
                });

                return new PriceAndAvailabilityRequestType
                {
                    Version = "5.4.0",
                    Header = new HeaderType
                    {
                        ThisDocumentIdentifier = new ThisDocumentIdentifierType
                        {
                            DocumentIdentifier = palRequest.MessageId
                        },
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
                                PartnerName = new string[] { palRequest.Location.Name },
                                PartnerIdentifier = new PartnerIdentifierType[]
                                {
                                    new PartnerIdentifierType
                                    {
                                        Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                        Value = palRequest.Location.AgrimineEBID
                                    }
                                },
                                ContactInformation = new ContactInformationType[]
                                {
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
                                        ContactName = new string[] { palRequest.SeedYear },
                                        ContactDescription = new string[] { "SeedYear" }
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
                                PartnerName = new string[] { palRequest.Vendor.Name },
                                PartnerIdentifier = new PartnerIdentifierType[]
                                {
                                    new PartnerIdentifierType
                                    {
                                        Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                        Value = palRequest.Vendor.GLN
                                    }
                                }
                            }
                        }
                    },
                    PriceAndAvailabilityRequestBody = new PriceAndAvailabilityRequestBodyType
                    {
                        PriceAndAvailabilityRequestProperties = new PriceAndAvailabilityRequestPropertiesType
                        {
                            RequisitionNumber = new RequisitionNumberType
                            {
                                DocumentIdentifier = palRequest.MessageId
                            },
                            RequisitionTypeCode = new RequisitionTypeCodeType
                            {
                                Domain = "ANSI-ASC-X12-92",
                                Value = "DEALRPAL"
                            },
                            LanguageCode = new LanguageCodeType
                            {
                                Domain = "ISO-639-2T",
                                Value = "ENG"
                            },
                            CurrencyCode = new CurrencyCodeType
                            {
                                Domain = "ISO-4217",
                                Value = "USD"
                            }
                        },
                        PriceAndAvailabilityRequestPartners = new PriceAndAvailabilityRequestPartnersType
                        {
                            Buyer = new BuyerType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { palRequest.Location.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = palRequest.Location.AgrimineEBID
                                        }
                                    }
                                }
                            },
                            Seller = new SellerType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { palRequest.Vendor.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = palRequest.Vendor.GLN
                                        }
                                    }
                                }
                            },
                            ShipTo = new ShipToType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { palRequest.Location.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = palRequest.Location.AgrimineEBID
                                        }
                                    }
                                }
                            },
                            Payer = new PayerType
                            {
                                PartnerInformation = new PartnerInformationType
                                {
                                    PartnerName = new string[] { palRequest.Payer.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = palRequest.Payer.PartnerIdentifier
                                        }
                                    }
                                }
                            }
                        },
                        PriceAndAvailabilityRequestDetails = mappedRequestLineItems.ToArray()
                    }
                };
            });
        }


    }
}

