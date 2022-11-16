using System;

namespace SuperSimpleTcp;

public class UserEventArgs : EventArgs
{
    /// <summary>
    /// Arguments for authorize events.
    /// </summary>
    internal UserEventArgs(string ipPort, string nick)
    {
        IpPort = ipPort;
        Nick = nick;
    }

    /// <summary>
    /// The IP address and port number of the connected peer socket.
    /// </summary>
    public string IpPort { get; }

    /// <summary>
    /// The nick of the connected peer socket.
    /// </summary>
    public string Nick { get; }
}