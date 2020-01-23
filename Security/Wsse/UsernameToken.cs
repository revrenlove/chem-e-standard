using System.Runtime.Serialization;

namespace EFC.AgGateway.Integration.Security.Wsse
{
    /// <summary>
    /// Data contract for credentials to be supplied in the Security object
    /// </summary>
    [DataContract(Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class UsernameToken
    {
        [DataMember(Order = 0)]
        public string Username { get; set; }
        [DataMember(Order = 1)]
        public string Password { get; set; }
    }
}
