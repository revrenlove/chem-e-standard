using EFC.Data.Merchant.DTOs.AccountsPayable.EFulfillment;
using EFC.Data.Merchant.Repositories.AccountsPayable.EFulfillment;
using EFC.Foundation.Data;
using EFC.Merchant.Enumerations.AccountsPayable.EFulfillment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC.AgGateway.Integration
{
    public class MessageEndPointService : IMessageEndPointService
    {
        IDataWorker _dataWorker;
        IVendorEFulfillmentMessagesRepository _vendorEFulfillmentMessagesRepository;

        public string GetMessageEndpoint(string vendorId, EFulfillmentMessageType messageType)
        {
            var filter = new VendorEFulfillmentMessage
            {
                VendorID = vendorId,
                Type = messageType
            };

            var vendorEFulfillmentMessage = _vendorEFulfillmentMessagesRepository.GetVendorEFulfillmentMessage(filter, _dataWorker);

            return vendorEFulfillmentMessage.FolderEndpoint;
        }

        public MessageEndPointService(IVendorEFulfillmentMessagesRepository vendorEFulfillmentMessagesRepository, IDataWorker dataWorker)
        {
            _dataWorker = dataWorker;
            _vendorEFulfillmentMessagesRepository = vendorEFulfillmentMessagesRepository;
        }
    }
}
