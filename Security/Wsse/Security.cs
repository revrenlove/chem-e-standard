using System.Runtime.Serialization;

namespace EFC.AgGateway.Integration.Security.Wsse
{
    /// <summary>
    /// Data contract for the Security object needed for all messages using Wsse authentication
    /// </summary>
    [DataContract(Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class Security
    {
        public Security()
        {

        }

        public Security(string username, string password)
        {
            UsernameToken = new UsernameToken
            {
                Username = username,
                Password = password
            };
        }

        public Security(UsernameToken usernameToken)
        {
            UsernameToken = usernameToken;
        }

        [DataMember]
        public UsernameToken UsernameToken { get; set; }
    }
}
