using Nini.Config;

namespace AuthServer.Config.Configs
{
    public class ConfigNetwork
    {
        IConfigSource source = new IniConfigSource("config\\network.ini");

        public string BindIp = string.Empty;
        public short BindPort = 0;

        public ConfigNetwork()
        {
            BindIp = source.Configs["Network"].GetString("Bind_Ip");
            BindPort = (short)source.Configs["Network"].GetInt("Bind_Port");
        }
    }
}
