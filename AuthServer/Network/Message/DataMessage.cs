using Hik.Communication.Scs.Communication.Messages;

namespace AuthServer.Network.Message
{
    public class DataMessage : ScsMessage
    {
        public short OpCode;

        public byte[] Data;
    }
}
