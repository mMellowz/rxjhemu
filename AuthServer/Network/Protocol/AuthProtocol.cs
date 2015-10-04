using AuthServer.Network.Message;
using common;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Communication.Protocols;
using System;
using System.Collections.Generic;
using System.IO;

namespace AuthServer.Network.Protocol
{
    public class AuthProtocol : IScsWireProtocol
    {
        protected MemoryStream Stream = new MemoryStream();

        public byte[] GetBytes(IScsMessage message)
        {
            return ((DataMessage)message).Data;
        }

        public IEnumerable<IScsMessage> CreateMessages(byte[] receivedBytes)
        {
            Stream.Write(receivedBytes, 0, receivedBytes.Length);

            List<IScsMessage> messages = new List<IScsMessage>();

            while (ReadMessage(messages)) ;

            return messages;
        }

        private bool ReadMessage(List<IScsMessage> messages)
        {
            Stream.Position = 0;

            if (Stream.Length < 4)
                return false;

            byte[] opcode = new byte[2];
            byte[] dlen = new byte[2];

            Stream.Read(opcode, 0, 2);
            Stream.Read(dlen, 0, 2);

            int length = BitConverter.ToUInt16(dlen, 0);

            if ((Stream.Length - 4) < length)
                return false;

            DataMessage message = new DataMessage
            {
                OpCode = BitConverter.ToInt16(opcode, 0),
                Data = new byte[length]
            };

            Stream.Read(message.Data, 0, message.Data.Length);

            messages.Add(message);

            TrimStream();

            return true;
        }

        private void TrimStream()
        {
            if (Stream.Position == Stream.Length)
            {
                Stream = new MemoryStream();
                return;
            }

            byte[] remaining = new byte[Stream.Length - Stream.Position];
            Stream.Read(remaining, 0, remaining.Length);
            Stream = new MemoryStream();
            Stream.Write(remaining, 0, remaining.Length);
        }

        

        public void Reset()
        {
           
        }
    }
}
