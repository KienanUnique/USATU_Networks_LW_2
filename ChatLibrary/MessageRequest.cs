namespace ChatLibrary;

public class MessageRequest
{
    public string Nick { get; }
    public string Message { get; }

    public MessageRequest(string nick, string message)
    {
        Nick = nick;
        Message = message;
    }
}