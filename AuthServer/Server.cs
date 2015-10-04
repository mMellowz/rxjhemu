using AuthServer.Config;
using AuthServer.Network;
using common;
using System;
using System.Diagnostics;
using System.Reflection;

namespace AuthServer
{
    class Server
    {
        static void Main(string[] args)
        {
            InitInfo();
            StartServer();
        }

        static void StartServer()
        {
            ConfigManager.Init();
            NetOpcode.Init();

            new NetListener();
            
            Process.GetCurrentProcess().WaitForExit();
        }

        static void InitInfo()
        {
            var asmb = Assembly.GetExecutingAssembly();
            Console.Title = asmb.GetName().Name + " - " + asmb.GetName().Version;
        }
    }
}
