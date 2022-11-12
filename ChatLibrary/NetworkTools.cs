using System.Net;
using System.Text.Json;

namespace ChatLibrary;

public static class NetworkTools
{
    public static string GetStringJsonSendMessage(PocketTCP pocketTcp)
    {
        return JsonSerializer.Serialize(pocketTcp);
    }

    public static PocketTCP GetPocketTcpFromJson(string json)
    {
        return new PocketTCP(JsonSerializer.Deserialize<ParsedPocketTCP>(json));
    }
    public static bool IsAddressAndPortCorrect(string ipAddressToCheck, string portToCheck)
    {
        bool isPortCorrect = int.TryParse(portToCheck, out var port);
        bool isIpCorrect = IPAddress.TryParse(ipAddressToCheck, out var address);
        return isPortCorrect && isIpCorrect;
    }
}