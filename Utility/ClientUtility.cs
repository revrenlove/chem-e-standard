using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using EFC.AgGateway.Integration.Exceptions;
using EFC.AgGateway.Integration.Specifications.XmlSoap;

namespace EFC.AgGateway.Integration.Utility
{
    public static class ClientUtility
    {
        /// <summary>
        /// This handles building the EndpointAddress given 3 possible cases:
        ///     1) Only a baseUrl is provided.
        ///     2) Only an endpoint is provided (and contains a full URL)
        ///     3) Both are provided
        /// </summary>
        /// <param name="baseUrl">The base URL of the web service</param>
        /// <param name="endpoint">The (optional) endpoint to be appended to the base URL.</param>
        /// <returns></returns>
        public static EndpointAddress BuildEndpointAddress(string baseUrl, string endpoint)
        {
            if (!IsValidBaseUrlAndEndpoint(baseUrl, endpoint))
            {
                var argumentException = new ArgumentException($"{nameof(baseUrl)} value: {baseUrl}. {nameof(endpoint)} value: {endpoint}");

                var message =
                    $"Unable to create EndpointAddress from values supplied for " +
                    $"{nameof(baseUrl)} and {nameof(endpoint)}. " +
                    "See Inner Exception for values provided.";

                throw new AgGatewayException(message, argumentException);
            }

            string uri;

            if (string.IsNullOrEmpty(baseUrl))
            {
                uri = endpoint;
            }
            else if (string.IsNullOrEmpty(endpoint))
            {
                uri = baseUrl;
            }
            else
            {
                baseUrl = Regex.Replace(baseUrl, "/$", string.Empty);
                endpoint = Regex.Replace(endpoint, "^/", string.Empty);

                uri = $"{baseUrl}/{endpoint}";
            }

            return new EndpointAddress(uri);
        }

        /// <summary>
        /// Based upon the base url, this determines the appropriate binding to use
        /// </summary>
        /// <returns>The Binding object</returns>
        public static Binding GetBinding(EndpointAddress endpointAddress)
        {
            HttpBindingBase binding;

            if (endpointAddress.Uri.Scheme == "https")
            {
                binding = new BasicHttpsBinding();
            }
            else
            {
                binding = new BasicHttpBinding();
            }

            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.SendTimeout = TimeSpan.FromMinutes(5);

#if DEBUG
            //Visual Studio debugger will only execute code for the current thread. However, this property requires cross-thread interaction to be executed and
            //will receive a "The function evaulation requires all threads to run" error by the debugger.
            //The property is being invoked here to avoid forcing the developer to have to manually evaluate this by way of the debugger each time they run this code.
            var messageVersion = binding.MessageVersion;
#endif
            return binding;
        }

        /// <summary>
        /// Parses the Fault object from the XML response within the protocol exception
        /// and generates a new exception with that information.
        /// </summary>
        /// <param name="ex">Protocol exception thrown</param>
        /// <returns>An AgGatewayException with applicable information.</returns>
        public static AgGatewayCommunicationException HandleFault(CommunicationException ex)
        {
            var webException = ex.InnerException as WebException;
            string responseBody;

            using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
            {
                responseBody = streamReader.ReadToEnd();
            }

            var soapEnvelopeXml = XmlUtility.Serialize(responseBody);

            var soapEnvelope = XmlUtility.Deserialize<Envelope>(soapEnvelopeXml);

            var faultXml = soapEnvelope.Body.Any[0];

            try
            {
                var fault = XmlUtility.Deserialize<Fault>(faultXml);

                return new AgGatewayCommunicationException("Error making request. See Fault and inner exception for more details...", ex, fault);
            }
            catch (InvalidOperationException)
            {
                return new AgGatewayCommunicationException("Unable to parse Fault. See inner exception for details...", ex);
            }
        }

        // TODO: JE - Document this...
        public static AgGatewayCommunicationException HandleFault(Message message)
        {
            var responseBody = message.ToString();

            var soapEnvelopeXml = XmlUtility.Serialize(responseBody);

            var soapEnvelope = XmlUtility.Deserialize<Envelope>(soapEnvelopeXml);

            var faultXml = soapEnvelopeXml.GetElementsByTagName("Fault", "http://schemas.xmlsoap.org/soap/envelope/").Item(0);

            try
            {
                var fault = XmlUtility.Deserialize<Fault>(faultXml);

                return new AgGatewayCommunicationException("Error making request. See Fault for more details...", fault);
            }
            catch (InvalidOperationException)
            {
                return new AgGatewayCommunicationException("Unable to parse Fault.");
            }
        }

        /// <summary>
        /// Tests to ensure that exactly one of the parameters provided contains the
        ///     beginning of the URL
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private static bool IsValidBaseUrlAndEndpoint(string baseUrl, string endpoint)
        {
            var urlPattern = "^http";

            var isBaseUrlStartOfUrl =
                !string.IsNullOrEmpty(baseUrl) && 
                Regex.IsMatch(baseUrl, urlPattern);

            var isEndpointStartOfUrl =
                !string.IsNullOrEmpty(endpoint) &&
                Regex.IsMatch(endpoint, urlPattern) &&
                string.IsNullOrEmpty(baseUrl);

            // The `^` is a logical XOR operator.
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-exclusive-or-operator-
            return isBaseUrlStartOfUrl ^ isEndpointStartOfUrl;
        }
    }
}
