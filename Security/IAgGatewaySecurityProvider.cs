using System;
using System.ServiceModel;

namespace EFC.AgGateway.Integration.Security
{
    public interface IAgGatewaySecurityProvider
    {
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
        TResponse ExecuteRequest<TClient, TRequest, TResponse>(ClientBase<TClient> client, TRequest request, Func<TRequest, TResponse> requestDelegate)
            where TClient : class
            where TRequest : class
            where TResponse : class;
    }
}
