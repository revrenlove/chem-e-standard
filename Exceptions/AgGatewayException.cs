using System;

namespace EFC.AgGateway.Integration.Exceptions
{
    /// <summary>
    /// Generic AgGateway exception
    /// </summary>
    public class AgGatewayException : Exception
    {
        public AgGatewayException()
        {
        }

        public AgGatewayException(string message) : base(message)
        {
        }

        public AgGatewayException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
