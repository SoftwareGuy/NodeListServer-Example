// This file is part of the NodeListServer Example package.
using System;
using System.Collections.Generic;

namespace NodeListServer
{

    [Serializable]
    public class ServerListResponse
    {
        // Number of known servers.
        public int count;
        // The container for the known servers.
        public List<ServerListEntry> servers;
        // Ideally used for client-hosted games, tells you how often you should refresh your server information.
        public int updateFrequency;
    }

    [Serializable]
    public class ServerListEntry
    {
        // IP address. Beware: Might be IPv6 format, and require you to chop off the leading "::ffff:" part. YMMV.
        public string ip;
        // Name of the server.
        public string name;
        // Port of the server.
        public int port;
        // Number of players on the server.
        public int players;
        // The number of players maximum allowed on the server.
        public int capacity;
        // Extra data.
        public string extras;
    }

    [Serializable]
    public struct ServerInfo
    {
        public string Name;         // The name of the server.
        // public string Ip;           // The IP of the server.
        public int Port;            // The port of the server.
        public int Count;           // The count of players currently on the server.
        public int Capacity;        // The count of players allowed on the server.
        public string ExtraInformation; // Some extra information, probably best in JSON format for easy parsing.
    }
}