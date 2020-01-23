using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EFC.AgGateway.Integration.MessageInspection;

namespace EFC.AgGateway.Integration.Security.Wsse
{
    /// <summary>
    /// Security provider for any AgGateway service using Wsse security
    /// </summary>
    public class WsseSecurityProvider : AgGatewaySecurityProviderBase, IAgGatewaySecurityProvider
    {
        private readonly string _username;
        private readonly string _password;

        public WsseSecurityProvider(string username, string password)
        {
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Executes the actual web request while applying appropriate security credentials
        /// </summary>
        /// <typeparam name="TClient">The type of client being called</typeparam>
        /// <typeparam name="TRequest">The type of request being made</typeparam>
        /// <typeparam name="TResponse">The type of response received</typeparam>
        /// <param name="client">The client to use to make the service call</param>
        /// <param name="request">The request object being sent</param>
        /// <param name="requestDelegate">The method to use to actually make the call from the provided client</param>
        /// <returns></returns>
        public TResponse ExecuteRequest<TClient, TRequest, TResponse>(ClientBase<TClient> client, TRequest request, Func<TRequest, TResponse> requestDelegate)
            where TClient : class
            where TRequest : class
            where TResponse : class
        {
            client.Endpoint.EndpointBehaviors.Add(new InvalidResponseMessageHandlingBehavior());

            var securityHeader = CreateSecurityHeader(_username, _password);

            using (var operationContextScope = new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(securityHeader);

                return ExecuteRequest(request, requestDelegate);
            }
        }

        private MessageHeader CreateSecurityHeader(string username, string password)
        {
            var security = new Security(username, password);

            var serializer = new DataContractSerializer(typeof(Security));

            return MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", security, serializer);
        }
    }
}
