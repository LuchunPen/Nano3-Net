/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08.06.2016 19:40:13
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using Nano3.Collection;

namespace Nano3.Net
{
    public static class NetHelper
    {
        public const int READ_BUFFER_SMALL = 2048;
        public const int READ_BUFFER_NORMAL = 8192;
        public const int READ_BUFFER_BIG = 32178;
        public const int READ_BUFFER_LARGE = 65536;

        public static readonly IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);

        public static bool CheckPort(int port)
        {
            if (port < 0 || port > 65535) return false;
            return true;
        }

        public static IPEndPoint GetIPEndPoint(string IP, string port)
        {
            if (string.IsNullOrEmpty(IP)) { throw new ArgumentNullException("IP"); }
            if (string.IsNullOrEmpty(port)) { throw new ArgumentNullException("port"); }

            IPAddress ip; IPAddress.TryParse(IP, out ip);
            int p; int.TryParse(port, out p);

            if (ip == null) { throw new ArgumentNullException("IP"); }
            if (!CheckPort(p)) { throw new ArgumentOutOfRangeException("Port"); }

            return new IPEndPoint(ip, p);
        }

        public static IPEndPoint GetIPEndPoint(IPAddress IP, int port)
        {
            if (IP == null) { throw new ArgumentNullException("IP"); }
            if (!CheckPort(port)) { throw new ArgumentOutOfRangeException("Port"); }

            return new IPEndPoint(IP, port);
        }

        public static IPAddress FindMyIP4()
        {
            string thisHostName = "";
            IPHostEntry thisHostDNSEntry = null;
            IPAddress[] allIPs = null;
            IPAddress myIP4 = null;

            try
            {
                thisHostName = Dns.GetHostName();
                thisHostDNSEntry = Dns.GetHostEntry(thisHostName);
                allIPs = thisHostDNSEntry.AddressList;
                for (int idx = allIPs.Length - 1; idx > 0; idx--)
                {
                    if (allIPs[idx].AddressFamily == AddressFamily.InterNetwork)
                    {
                        myIP4 = allIPs[idx];
                        break;
                    }
                }
            }
            catch { return null; }
            return myIP4;
        }

        public static int GetFreePort()
        {
            RandomXor rand = new RandomXor();

            int portStartIndex = 32768;
            int portEndIndex = 65000;

            IPGlobalProperties nProps = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tps = nProps.GetActiveTcpListeners();
            IPEndPoint[] ups = nProps.GetActiveUdpListeners();

            FastHashSetM2<int> usedPorts = new FastHashSetM2<int>();
            for (int i = 0; i < tps.Length; i++){
                usedPorts.Add(tps[i].Port);
            }
            for (int i = 0; i < ups.Length; i++){
                usedPorts.Add(ups[i].Port);
            }

            GETNEWPORT:
            int freeport = rand.Next(portStartIndex, portEndIndex);
            if (usedPorts.Contains(freeport)) { goto GETNEWPORT; }
            return freeport;
        }

        public static IPEndPoint Snapshot(this IPEndPoint ep)
        {
            if (ep != null) { return new IPEndPoint(ep.Address, ep.Port); }
            else return null;
        }
    }
}
