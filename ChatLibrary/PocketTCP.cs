using System;

namespace ChatLibrary;

public enum RequestsTypes
{
    Message,
    LogIn,
    SignUp,
    NewUserAuthorize,
    AnswerOnAuthentication,
    Disconnection
}

public enum AnswerOnAuthenticationTypes // TODO: Add different auth answers
{
    Ok,
    UserWithSuchNickAlreadyExist,
    SessionForThisUserAlreadyExist,
    IncorrectLoginInformation
}

public class SenderInfo
{
    public string Nick { get; }
    public string IpPort { get; }

    public SenderInfo(string nick, string ipPort)
    {
        Nick = nick;
        IpPort = ipPort;
    }
    
    public override bool Equals(System.Object obj)
    {
        if (obj == null)
        {
            return false;
        }
        
        var p = obj as SenderInfo;
        if ((System.Object)p == null)
        {
            return false;
        }
        
        return (Nick == p.Nick) && (IpPort == p.IpPort);
    }
}

public class ParsedPocketTCP
{
    public string SenderIpPort { get; }
    public string SenderNick { get; }
    public int RequestType { get; }
    public string Message { get; }

    public ParsedPocketTCP(string senderNick, string senderIpPort, int requestType, string message)
    {
        SenderNick = senderNick;
        SenderIpPort = senderIpPort;
        RequestType = requestType;
        Message = message;
    }
}

public class PocketTCP
{
    public string SenderIpPort { get; }
    public string SenderNick { get; }
    public RequestsTypes RequestType { get; }
    public string Message { get; }

    public PocketTCP(string senderNick, string senderIpPort, RequestsTypes requestType, string message)
    {
        SenderNick = senderNick;
        SenderIpPort = senderIpPort;
        RequestType = requestType;
        Message = message;
    }

    public PocketTCP(string senderNick, string senderIpPort, RequestsTypes requestType)
    {
        SenderNick = senderNick;
        SenderIpPort = senderIpPort;
        RequestType = requestType;
        Message = String.Empty;
    }

    public PocketTCP(ParsedPocketTCP parsedPocketTcp) : this(parsedPocketTcp.SenderNick, parsedPocketTcp.SenderIpPort,
        (RequestsTypes) parsedPocketTcp.RequestType, parsedPocketTcp.Message)
    {
    }
}