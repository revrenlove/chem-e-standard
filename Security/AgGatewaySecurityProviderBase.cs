using System;
using System.ServiceModel;
using EFC.AgGateway.Integration.MessageInspection;
using EFC.AgGateway.Integration.Utility;

namespace EFC.AgGateway.Integration.Security
{
    public abstract class AgGatewaySecurityProviderBase
    {
        protected internal AgGatewaySecurityProviderBase() { }

        /// <summary>
        /// TODO: JE - actually fill this out...
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="requestDelegate"></param>
        /// <returns></returns>
        protected TResponse ExecuteRequest<TRequest, TResponse>(TRequest request, Func<TRequest, TResponse> requestDelegate)
            where TRequest : class
            where TResponse : class
        {
            try
            {
                var response = requestDelegate(request);

                if (response == null)
                {
                    throw ClientUtility.HandleFault(ResponseMessageHolder.GetMessage());
                }

                return response;
            }
            catch (CommunicationException ex)
            {
                throw ClientUtility.HandleFault(ex);
            }
        }
    }
}
