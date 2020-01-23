using System.ServiceModel.Channels;

namespace EFC.AgGateway.Integration.MessageInspection
{
    public static class ResponseMessageHolder
    {
        private static Message _message;

        public static void SetMessage(Message message)
        {
            _message = message;
        }

        public static Message GetMessage()
        {
            return _message;
        }
    }
}
