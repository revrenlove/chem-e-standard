using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using EFC.AgGateway.Integration.Models;
using EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract;
using EFC.Data.Merchant.DTOs.Inventory;

namespace EFC.AgGateway.Integration.Mapping.Profiles
{
    /// <summary>
    /// Mapping profile for all objects related to Price Sheet requests
    /// </summary>
    public class PriceSheetProfile : Profile
    {
        public PriceSheetProfile()
        {
            MapPriceSheetRequest();
            MapPriceSheetProductLineItemToImportSeedItem();
            MapPriceSheetProductLineItemToImportSeedItemZone();
            CreateMap<PriceSheetType, IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ImportSeedItemZone)>>()
                .ConvertUsing(new PriceSheetResponseConverter());
        }

        private void MapPriceSheetRequest()
        {
            CreateMap<PriceSheetRequest, PriceSheetRequestType>().ConvertUsing(priceSheetRequest =>
            {
                return new PriceSheetRequestType
                {
                    Header = new HeaderType3
                    {
                        partnerId = priceSheetRequest.Location.AgrimineEBID,
                        partnerType = "AGIIS-EBID",
                        messageId = Guid.NewGuid().ToString()
                    },
                    PriceSheetRequestDateTimeRange = new DateTimeRangeType
                    {
                        FromDateTime = priceSheetRequest.LastRequestDate.ToUniversalTime(),
                        ToDateTime = DateTime.UtcNow
                    },
                    PriceSheetRequest = new PriceSheetRequestType1
                    {
                        Header = new HeaderType1
                        {
                            ThisDocumentIdentifier = new ThisDocumentIdentifierType
                            {
                                DocumentIdentifier = priceSheetRequest.MessageId
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
                                    PartnerName = new string[] { priceSheetRequest.Location.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = priceSheetRequest.Location.AgrimineEBID
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
                                            ContactName = new string[] { priceSheetRequest.SeedYear },
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
                                    PartnerName = new string[] { priceSheetRequest.Vendor.Name },
                                    PartnerIdentifier = new PartnerIdentifierType[]
                                    {
                                        new PartnerIdentifierType
                                        {
                                            Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                            Value = priceSheetRequest.Vendor.GLN
                                        }
                                    }
                                }
                            }
                        },
                        Version = "5.4.0",
                        PriceSheetRequestBody = new PriceSheetRequestBodyType
                        {
                            PriceSheetRequestProperties = new RetailerOrderSummaryReportPropertiesType
                            {
                                CurrencyCode = new CurrencyCodeType
                                {
                                    Domain = "ISO-4217",
                                    Value = "USD"
                                },
                                LanguageCode = new LanguageCodeType
                                {
                                    Domain = "ISO-639-2T",
                                    Value = "eng"
                                },
                                LastRequestDate = new LastRequestDateType
                                {
                                    DateTime = new DateTimeType
                                    {
                                        DateTimeQualifier = cidxListDateQualifier.On,
                                        Value = priceSheetRequest.LastRequestDate
                                    }
                                },
                                ZoneID = priceSheetRequest.ZoneId,
                                ProductClassification = priceSheetRequest.CropCode
                            },
                            PriceSheetRequestPartners = new PriceSheetRequestPartnersType
                            {
                                Buyer = new BuyerType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { priceSheetRequest.Location.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,

                                                Value = priceSheetRequest.Location.AgrimineEBID
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            });
        }

        private void MapPriceSheetProductLineItemToImportSeedItem()
        {
            CreateMap<PriceSheetProductLineItemType, ImportSeedItem>().ConvertUsing(productLineItem =>
            {
                var vendorItemId = productLineItem.GetIdByAgency(cidxListProductIDAgency.AssignedBySeller);
                var upc = productLineItem.GetIdByAgency(cidxListProductIDAgency.UPC);
                var gtin = productLineItem.GetIdByAgency(cidxListProductIDAgency.AGIISProductID);

                var description = productLineItem.GetDescriptionByAgency(cidxListProductIDAgency.AGIISProductID);
                var cropCode = productLineItem.GetCropCodeByByAgency(cidxListProductIDAgency.AGIISProductID);

                var dealerPricingPerUnit = productLineItem.GetPricingPerUnitByType(cidxListPriceType.SuggestedDealerOrRetailerPrice);

                var trait = productLineItem.GetProductAttributeByAgencyAndName(cidxListTestMethodAgency.AssignedBySeller, "TRAIT");
                var brand = productLineItem.GetProductAttributeByAgencyAndName(cidxListTestMethodAgency.AssignedBySeller, "ACRONYMNAME");
                decimal.TryParse(productLineItem.GetProductAttributeByAgencyAndName(cidxListTestMethodAgency.AssignedBySeller, "RELATIVEMATURITY"), out decimal relativeMaturity);

                var importSeedItem = new ImportSeedItem
                {
                    VendorItemId = vendorItemId,
                    ItemDescription = description,
                    UPC = upc,
                    GTIN = gtin,
                    PUOM = dealerPricingPerUnit.PriceBasis.Measurement.UnitOfMeasureCode.Value,
                    CropCode = cropCode,
                    IsActive = true,
                    Trait = trait,
                    Brand = brand,
                    RelativeMaturity = relativeMaturity
                };

                return importSeedItem;
            });
        }

        private void MapPriceSheetProductLineItemToImportSeedItemZone()
        {
            CreateMap<PriceSheetProductLineItemType, ImportSeedItemZone>().ConvertUsing(productLineItem =>
            {
                var dealerPricingPerUnit = productLineItem.GetPricingPerUnitByType(cidxListPriceType.SuggestedDealerOrRetailerPrice);
                var growerPricingPerUnit = productLineItem.GetPricingPerUnitByType(cidxListPriceType.SuggestedGrowerOrEndUserPrice);

                var importSeedItemZone = new ImportSeedItemZone
                {
                    Cost = dealerPricingPerUnit.MonetaryAmount.MonetaryValue,
                    Price = growerPricingPerUnit.MonetaryAmount.MonetaryValue,
                    VendorItemId = productLineItem.GetIdByAgency(cidxListProductIDAgency.AssignedBySeller),
                    Zone = productLineItem.GetZone(),
                };

                return importSeedItemZone;
            });
        }

        private class PriceSheetResponseConverter
            : ITypeConverter<PriceSheetType, IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ImportSeedItemZone)>>
        {
            public IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ImportSeedItemZone)> Convert(
                PriceSheetType priceSheet,
                IEnumerable<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ImportSeedItemZone)> destination,
                ResolutionContext context)
            {
                var results = new List<(ImportSeedItem ImportSeedItem, ImportSeedItemZone ImportSeedItemZone)>();

                var seedYear = priceSheet.GetSeedYear();

                var lineItems = priceSheet.PriceSheet.PriceSheetBody.PriceSheetDetails.PriceSheetProductLineItem;

                foreach (var lineItem in lineItems)
                {
                    var importSeedItem = context.Mapper.Map<ImportSeedItem>(lineItem);
                    var importSeedItemZone = context.Mapper.Map<ImportSeedItemZone>(lineItem);

                    importSeedItem.SeedYear = seedYear;

                    results.Add((ImportSeedItem: importSeedItem, ImportSeedItemZone: importSeedItemZone));
                }

                return results;
            }
        }
    }
}
