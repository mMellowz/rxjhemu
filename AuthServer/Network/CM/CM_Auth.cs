using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network.CM
{
    public class CM_Auth : NetRecievePacket
    {
        protected string Username;

        protected string MD5Password;

        public override void Read()
        {
            Username = ReadS();
            MD5Password = ReadS();
        }

        public override void Process()
        {
            Log.Debug("\nUsername: {0}\nPassword: {1}", Username, MD5Password);
        }
    }
}
