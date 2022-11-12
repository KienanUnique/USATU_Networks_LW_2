using System;

namespace SuperSimpleTcp;

public class MessageReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Arguments for authorize events.
    /// </summary>
    internal MessageReceivedEventArgs(string ipPort, string nick, string message)
    {
        IpPort = ipPort;
        Nick = nick;
        Message = message;
    }

    /// <summary>
    /// The IP address and port number of the connected endpoint.
    /// </summary>
    public string IpPort { get; }

    /// <summary>
    /// Nick of the connected endpoint.
    /// </summary>
    public string Nick { get; }
    
    /// <summary>
    /// The message received from the endpoint.
    /// </summary>
    public string Message { get; }
}