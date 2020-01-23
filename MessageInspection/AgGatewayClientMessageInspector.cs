using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace EFC.AgGateway.Integration.MessageInspection
{
    public class AgGatewayClientMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //reply.ToHttpResponseMessage().StatusCode = System.Net.HttpStatusCode.InternalServerError;
            if (reply.IsFault)
            {

            }

            ResponseMessageHolder.SetMessage(reply);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }
    }
}
