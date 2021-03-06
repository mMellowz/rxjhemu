﻿using AuthServer.Config;
using common;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Server;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AuthServer.Network
{
    public class NetListener
    {
        public IScsServer Server;

        protected Dictionary<string, long> ConnectionsTime = new Dictionary<string, long>();

        public NetListener()
        {
            Log.Info("Start TcpServer at {0}:{1}...", ConfigManager.Network.BindIp, ConfigManager.Network.BindPort);
            Server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(ConfigManager.Network.BindIp, ConfigManager.Network.BindPort));
            Server.Start();

            Server.ClientConnected += OnConnected;
            Server.ClientDisconnected += OnDisconnected;
        }

        public void ShutdownServer()
        {
            Log.Info("Shutdown TcpServer...");
            Server.Stop();
        }

        protected void OnConnected(object sender, ServerClientEventArgs e)
        {
            string ip = Regex.Match(e.Client.RemoteEndPoint.ToString(), "([0-9]+).([0-9]+).([0-9]+).([0-9]+)").Value;

            // Skip web connect
            if (ip == "159.253.18.161")
                return;

            Log.Info("Client connected!");

            if (ConnectionsTime.ContainsKey(ip))
            {
                if (Funcs.GetCurrentMilliseconds() - ConnectionsTime[ip] < 2000)
                {
                    /*Process.Start("cmd",
                                  "/c netsh advfirewall firewall add rule name=\"AutoBAN (" + ip +
                                  ")\" protocol=TCP dir=in remoteip=" + ip + " action=block");
                    ConnectionsTime.Remove(ip);
                    Log.Info("TcpServer: FloodAttack prevent! Ip " + ip + " added to firewall");
                    return;*/
                }
                ConnectionsTime[ip] = Funcs.GetCurrentMilliseconds();
            }
            else
                ConnectionsTime.Add(ip, Funcs.GetCurrentMilliseconds());

            new NetSession(e.Client);
        }

        protected void OnDisconnected(object sender, ServerClientEventArgs e)
        {
            
        }
    }
}
