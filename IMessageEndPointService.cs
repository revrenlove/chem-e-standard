using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC.AgGateway.Integration
{
    public interface IMessageEndPointService
    {
        string GetMessageEndpoint(string vendorId, EFulfillmentMessageType messageType);
    }
}
