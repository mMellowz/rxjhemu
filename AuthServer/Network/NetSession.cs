using AuthServer.Network.Message;
using AuthServer.Network.Protocol;
using common;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;
using System;
using System.Collections.Generic;

namespace AuthServer.Network
{
    public class NetSession
    {
        public static List<NetSession> Sessions = new List<NetSession>();

        protected IScsServerClient Session;

        public byte[] Buffer;

        public NetSession(IScsServerClient client)
        {
            this.Session = client;
            this.Session.WireProtocol = new AuthProtocol();

            this.Session.Disconnected += OnDisconnected;
            this.Session.MessageReceived += OnMessageReceived;

            Log.Info("Create Session : " + this.Session.GetHashCode());
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            DataMessage message = (DataMessage)e.Message;
            Buffer = message.Data;

            if (NetOpcode.Recv.ContainsKey(message.OpCode))
            {
                ((NetRecievePacket)Activator.CreateInstance(NetOpcode.Recv[message.OpCode])).Process(this);
            }
            else
            {
                string opCodeLittleEndianHex = BitConverter.GetBytes(message.OpCode).ToHex();
                Log.Debug("Unknown Packet Opcode: 0x{0}{1} [Size: {2}]",
                                 opCodeLittleEndianHex.Substring(2),
                                 opCodeLittleEndianHex.Substring(0, 2),
                                 Buffer.Length);

                Log.Debug("Data:\n{0}", Buffer.FormatHex());
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Log.Info("Session {0} Disconnected...", this.Session.GetHashCode());
        }
    }
}
