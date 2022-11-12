using System.Net;
using System.Text.Json;

namespace ChatLibrary;

public static class NetworkTools
{
    public static string GetStringJsonSendMessage(MessageRequest messageRequest)
    {
        return JsonSerializer.Serialize(messageRequest);
    }

    public static MessageRequest GetMessageRequestFromJson(string json)
    {
        return JsonSerializer.Deserialize<MessageRequest>(json);
    }
    public static bool IsAddressAndPortCorrect(string ipAddressToCheck, string portToCheck)
    {
        bool isPortCorrect = int.TryParse(portToCheck, out var port);
        bool isIpCorrect = IPAddress.TryParse(ipAddressToCheck, out var address);
        return isPortCorrect && isIpCorrect;
    }
}