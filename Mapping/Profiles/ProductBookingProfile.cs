using AutoMapper;
using EFC.AgGateway.Integration.Models;
using EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract;
using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EFC.AgGateway.Integration.Mapping.Profiles
{
    public class ProductBookingProfile : Profile
    {
        public ProductBookingProfile()
        {
            MapProductBookingRequest();
            MapProductBookingResponse();
        }

        private void MapProductBookingRequest()
        {
            CreateMap<ProductBookingRequest, ProductBookingType>().ConvertUsing(productBookingRequest =>
            {
                return new ProductBookingType
                {
                    Header = new HeaderType
                    {
                        ThisDocumentIdentifier = new ThisDocumentIdentifierType
                        {
                            DocumentIdentifier = Guid.NewGuid().ToString()
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
                                PartnerName = new string[] { productBookingRequest.Location.Name },
                                PartnerIdentifier = new PartnerIdentifierType[]
                                {
                                    new PartnerIdentifierType
                                    {
                                        Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                        Value = productBookingRequest.Location.AgrimineEBID
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
                                        ContactName = new string[] { productBookingRequest.SeedYear },
                                        ContactDescription = new string[] { "SeedYear" }
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
                                PartnerName = new string[] { productBookingRequest.Vendor.Name },
                                PartnerIdentifier = new PartnerIdentifierType[]
                                {
                                    new PartnerIdentifierType
                                    {
                                        Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                        Value = productBookingRequest.Vendor.GLN
                                    }
                                }
                            }
                        }
                    },
                    ProductBookingBody = new ProductBookingBodyType[]
                    {
                        new ProductBookingBodyType
                        {
                            ProductBookingProperties = new ProductBookingPropertiesType
                            {
                                ProductBookingType = GetProductBookingType(productBookingRequest.BookingType),
                                ProductBookingOrderNumber = productBookingRequest.Booking.BookingId,
                                ProductBookingOrderTypeCode = new ProductBookingOrderTypeCodeType
                                {
                                    Domain = "ANSI-ASC-X12-92",
                                    Value ="KN"
                                },
                                ProductBookingOrderIssuedDate = new ProductBookingOrderIssuedDateType
                                {
                                    DateTime = new DateTimeType
                                    {
                                        DateTimeQualifier = cidxListDateQualifier.On,
                                        Value = DateTime.UtcNow
                                    }
                                },
                                LanguageCode = new LanguageCodeType
                                {
                                    Domain = "ISO-639-2T",
                                    Value = "EN"
                                },
                                CurrencyCode = new CurrencyCodeType
                                {
                                    Domain = "ISO-4217",
                                    Value = "USD"
                                },
                                BuyerSequenceNumber = "0",
                                SoftwareInformation = new SoftwareInformationType
                                {
                                    SoftwareSource = "MerchantAg",
                                    SoftwareVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                                },
                                ProductYear = productBookingRequest.SeedYear,
                                ReferenceInformation = new ReferenceInformationType[]
                                {
                                    new ReferenceInformationType
                                    {
                                        ReferenceType = cidxListReferenceType.SalesOrderReference,
                                        DocumentReference = new DocumentReferenceType
                                        {
                                            DocumentIdentifier = productBookingRequest.DocumentIdentifier
                                        }
                                    }
                                },
                                DirectShipFlag = IsDrugType.Item0
                            },
                            ProductBookingPartners = new ProductBookingPartnersType
                            {
                                Buyer = new BuyerType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { productBookingRequest.Location.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                                Value = productBookingRequest.Location.AgrimineEBID
                                            }
                                        },
                                        AddressInformation = new AddressInformationType
                                        { 
                                            ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.AddressLine },
                                            Items = new string[] { productBookingRequest.Location.Address },
                                            CityName = productBookingRequest.Location.City,
                                            StateOrProvince = productBookingRequest.Location.State,
                                            PostalCode = productBookingRequest.Location.Zip,
                                            PostalCountry = GetPostalCountry(productBookingRequest.Location.State)
                                        }
                                    }
                                },
                                Seller = new SellerType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { productBookingRequest.Vendor.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                                Value = productBookingRequest.Vendor.GLN
                                            }
                                        }
                                    }
                                },
                                ShipTo = new ShipToType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { productBookingRequest.Customer.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AssignedBySeller,
                                                Value = GetCustomerIdentifier(productBookingRequest)
                                            }
                                        },
                                        ContactInformation = new ContactInformationType[]{},
                                        AddressInformation = new AddressInformationType
                                        {
                                            ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.AddressLine },
                                            Items = new string[] { productBookingRequest.Customer.Address1 },
                                            CityName = productBookingRequest.Customer.City,
                                            StateOrProvince = productBookingRequest.Customer.State,
                                            PostalCode = productBookingRequest.Customer.Zip,
                                            PostalCountry = GetPostalCountry(productBookingRequest.Customer.State)
                                        }
                                    }
                                },
                                SoldTo = new SoldToType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { productBookingRequest.Location.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                                Value = productBookingRequest.Location.AgrimineEBID
                                            }
                                        }
                                    }
                                },
                                Payer = new PayerType
                                {
                                    PartnerInformation = new PartnerInformationType
                                    {
                                        PartnerName = new string[] { productBookingRequest.Payer.Name },
                                        PartnerIdentifier = new PartnerIdentifierType[]
                                        {
                                            new PartnerIdentifierType
                                            {
                                                Agency = cidxListPartnerAgencyAttribute.AGIISEBID,
                                                Value = productBookingRequest.Payer.PartnerIdentifier
                                            }
                                        }
                                    }
                                }
                            },
                            ProductBookingDetails = new ProductBookingProductLineItemType[]
                            {
                                new ProductBookingProductLineItemType
                                {
                                    LineNumber = Convert.ToUInt32(productBookingRequest.LineNumber),
                                    ActionRequest = GetActionRequestType(productBookingRequest.BookingType, productBookingRequest.TotalQuantity),
                                    LineItemType = "Sale",
                                    ProductBookingOrderLineItemNumber = "1",
                                    ProductIdentification = new ProductIdentificationType
                                    {
                                        ProductIdentifier = new ProductIdentifierType
                                        {
                                            Agency = cidxListProductIDAgency.AGIISProductID,
                                            Value = productBookingRequest.Item.GlobalTradeItemNumber
                                        }
                                    },
                                    ReferenceInformation = new ReferenceInformationType
                                    {
                                        ReferenceType = cidxListReferenceType.ContractNumber,
                                        DocumentReference = new DocumentReferenceType
                                        {
                                            DocumentIdentifier = $"{productBookingRequest.Booking.CustomerId}-{productBookingRequest.Booking.PlanningId}"
                                        }
                                    },
                                    IncreaseOrDecrease = new IncreaseOrDecreaseType
                                    {
                                        IncreaseOrDecreaseType1 = GetIncreaseOrDecreaseType(productBookingRequest.QuantityChangedAmount),
                                        ProductQuantityChange = new ProductQuantityChangeType
                                        {
                                            Measurement = new MeasurementType
                                            {
                                                MeasurementValue = (double)Math.Abs(productBookingRequest.QuantityChangedAmount),
                                                UnitOfMeasureCode = new UnitOfMeasureCodeType
                                                {
                                                    Domain = "UN-Rec-20",
                                                    Value = productBookingRequest.UNRec20UnitOfMeasure
                                                }
                                            }
                                        }
                                    },
                                    ProductQuantity = new ProductQuantityType
                                    {
                                        Measurement = new MeasurementType
                                        {
                                            MeasurementValue = (double)productBookingRequest.TotalQuantity,
                                            UnitOfMeasureCode = new UnitOfMeasureCodeType
                                            {
                                                Domain = "UN-Rec-20",
                                                Value = productBookingRequest.UNRec20UnitOfMeasure
                                            }
                                        }
                                    },
                                    RequestedShipDateTime = new RequestedShipDateTimeType
                                    {
                                        DateTimeInformation = new DateTimeInformationType
                                        {
                                            Item = new DateTimeType
                                            {
                                                DateTimeQualifier = cidxListDateQualifier.After,
                                                Value = DateTime.UtcNow
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

        private void MapProductBookingResponse()
        {
            CreateMap<ProductBookingResponseType, ProductBookingResponse>().ConvertUsing(productBookingResponseType =>
            {
                var bookingResponse = new ProductBookingResponse
                {
                    SalesOrderReferenceNumber = productBookingResponseType.GetSalesOrderReferenceNumber(),
                    LineItemNumber = productBookingResponseType.GetLineItemNumber(),
                    BookingId = productBookingResponseType.GetBookingId(),
                    DocumentIdentifier = productBookingResponseType.GetDocumentIdentifier(),
                    ProductIdentifier = productBookingResponseType.GetProductIdentifier(),
                    Quantity = Convert.ToDecimal(productBookingResponseType.GetQuantity()),
                    TimeStamp = productBookingResponseType.GetTimeStamp(),
                    PropertiesResponseDescriptions = productBookingResponseType.GetPropertiesResponseDescriptons(),
                    DetailsResponseDescriptions = productBookingResponseType.GetDetailsResponseDescriptions()
                };

                return bookingResponse;
            });
        }

        private string GetCustomerIdentifier(ProductBookingRequest request)
        {
            if (ProductBookingCustomerIdentifier.BayerTechLicense.Equals(request.CustomerIdentifier))
            {
                return !string.IsNullOrWhiteSpace(request.Customer.BayerTechLicense) ? request.Customer.BayerTechLicense : request.Customer.GlobalLocationNumber;
            }

            return request.Customer.GlobalLocationNumber;
        }

        private string GetPostalCountry(string stateAbbreviation)
        {
            var canadianProvinces = new List<string>() { "AB", "BC", "MB", "NB", "NF", "NS", "NT", "ON", "PE", "PQ", "QC", "SK", "YT" };

            if (canadianProvinces.Any(s => stateAbbreviation.Trim().ToUpper().Contains(s)))
            {
                return "Canada";
            }

            return "US";
        }

        private cidxListProductBookingType GetProductBookingType(Merchant.Enumerations.Orders.ProductBookingType productBookingType)
        {
            if (Merchant.Enumerations.Orders.ProductBookingType.New.Equals(productBookingType))
            {
                return cidxListProductBookingType.New;
            }

            if (Merchant.Enumerations.Orders.ProductBookingType.Changed.Equals(productBookingType))
            {
                return cidxListProductBookingType.Changed;
            }

            if (Merchant.Enumerations.Orders.ProductBookingType.Cancelled.Equals(productBookingType))
            {
                return cidxListProductBookingType.Cancelled;
            }

            return cidxListProductBookingType.SummaryRequest;
        }

        private cidxListIncreaseOrDecreaseType GetIncreaseOrDecreaseType(decimal quantityChangedAmount)
        {
            if (quantityChangedAmount < 0)
            {
                return cidxListIncreaseOrDecreaseType.Decrease;
            }

            return cidxListIncreaseOrDecreaseType.Increase;
        }

        private ActionRequestType GetActionRequestType(Merchant.Enumerations.Orders.ProductBookingType productBookingType, decimal quantityBooked)
        {
            if (Merchant.Enumerations.Orders.ProductBookingType.New.Equals(productBookingType))
            {
                return ActionRequestType.Add;
            }
            else
            {
                if (quantityBooked > 0)
                {
                    return ActionRequestType.Change;
                }
                else
                {
                    return ActionRequestType.Delete;
                }
            }
        }
    }
}
