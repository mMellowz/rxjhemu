using AuthServer.Network.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network
{
    public class NetOpcode
    {
        public static Dictionary<short, Type> Recv = new Dictionary<short, Type>();
        public static Dictionary<Type, short> Send = new Dictionary<Type, short>();

        public static Dictionary<short, string> SendNames = new Dictionary<short, string>();

        public static void Init()
        {
            Recv.Add(unchecked((short)0x8000), typeof(CM_Auth)); //1725 EU

            SendNames = Send.ToDictionary(p => p.Value, p => p.Key.Name);
        }
    }
}
