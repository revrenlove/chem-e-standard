using EFC.AgGateway.Integration.Exceptions;
using EFC.AgGateway.Integration.Security;
using EFC.AgGateway.Integration.Security.HttpHeader;
using EFC.AgGateway.Integration.Security.Wsse;
using EFC.Data.Merchant.DTOs.AccountsPayable;
using EFC.Foundation.Security;
using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;

namespace EFC.AgGateway.Integration
{
    /// <summary>
    /// Factory class to resolve appropriate AgGateway service
    ///    with appropriate security
    /// </summary>
    public static class AgGatewayServiceFactory
    {
        /// <summary>
        /// Gets the appropriate service to make/receive AgGateway
        ///     requests/responses based upon the vendor
        /// </summary>
        /// <param name="vendor">Vendor from which to determine the appropriate AgGateway Service</param>
        /// <returns>Appropriate AgGateway</returns>
        public static IAgGatewayService GetAgGatewayService(Vendor vendor)
        {
            var agGatewaySecurityProvider = GetSecurityProvider(vendor);

            return new AgGatewayService(agGatewaySecurityProvider, vendor.EFulfillUrl);
        }

        private static IAgGatewaySecurityProvider GetSecurityProvider(Vendor vendor)
        {
            var username = Cryptography.ExtendedDecrypt(vendor.OutboundEFulfillUserName, 128).Trim();
            var password = Cryptography.ExtendedDecrypt(vendor.OutboundEFulfillPassword, 128).Trim();

            IAgGatewaySecurityProvider agGatewaySecurityProvider;

            // TODO: JE - This is just a place holder until database changes go in.
            //            We will be defaulting to Wsse
            if (vendor.EFulfillmentSecurityType == null)
            {
                agGatewaySecurityProvider = new WsseSecurityProvider(username, password);
            }
            else if (vendor.EFulfillmentSecurityType == EFulfillmentSecurityType.Wsse)
            {
                agGatewaySecurityProvider = new WsseSecurityProvider(username, password);
            }
            else if (vendor.EFulfillmentSecurityType == EFulfillmentSecurityType.HttpHeader)
            {
                agGatewaySecurityProvider = new HttpHeaderSecurityProvider(username, password);
            }
            else
            {
                throw new AgGatewayException($"Invalid EFulfillmentSecurity type for vendor. VendorId: {vendor.Id}");
            }

            return agGatewaySecurityProvider;
        }
    }
}
