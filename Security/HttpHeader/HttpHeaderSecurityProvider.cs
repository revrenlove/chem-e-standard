using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EFC.AgGateway.Integration.MessageInspection;

namespace EFC.AgGateway.Integration.Security.HttpHeader
{
    /// <summary>
    /// Security provider for any AgGateway service passing credentials in the HTTP headers
    /// </summary>
    public class HttpHeaderSecurityProvider : AgGatewaySecurityProviderBase, IAgGatewaySecurityProvider
    {
        private readonly string _username;
        private readonly string _password;

        public HttpHeaderSecurityProvider(string username, string password)
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

            using (var operationContextScope = new OperationContextScope(client.InnerChannel))
            {
                var requestMessageProperty = new HttpRequestMessageProperty();

                requestMessageProperty.Headers["client_id"] = _username;
                requestMessageProperty.Headers["client_secret"] = _password;

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessageProperty;

                return ExecuteRequest(request, requestDelegate);
            }
        }
    }
}
