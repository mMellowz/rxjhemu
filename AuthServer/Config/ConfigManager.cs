using AuthServer.Config.Configs;

namespace AuthServer.Config
{
    public class ConfigManager
    {
        public static ConfigNetwork Network;

        public static void Init()
        {
            Network = new ConfigNetwork();
        }
    }
}
