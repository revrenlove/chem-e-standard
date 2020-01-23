using System;
using EFC.AgGateway.Integration.Specifications.XmlSoap;

namespace EFC.AgGateway.Integration.Exceptions
{
    public class AgGatewayCommunicationException : AgGatewayException
    {
        public AgGatewayCommunicationException()
        {
        }

        public AgGatewayCommunicationException(string message)
            : base(message)
        {
        }

        public AgGatewayCommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AgGatewayCommunicationException(string message, Exception innerException, Fault fault)
            : base(message, innerException)
        {
            Fault = fault;
        }

        public AgGatewayCommunicationException(string message, Fault fault)
            : base(message)
        {
            Fault = fault;
        }

        public Fault Fault { get; set; }
    }
}
