using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ChatLibrary;

public class ClientAlreadyExistException : Exception
{
    public ClientAlreadyExistException(string nick) : base($"Client with this nick already exists: {nick}")
    {
    }
}

public class ClientDoesntExistException : Exception
{
    public ClientDoesntExistException(string nick) : base($"Client with this nick doesn't exists: {nick}")
    {
    }
}

public class ClientsDataBase
{
    protected Dictionary<string, string> NicksAndPasswords = new();

    public bool IsClientWithSuchNickExist(string nick)
    {
        return NicksAndPasswords.ContainsKey(nick);
    }

    public void AddUser(string nick, string password)
    {
        if (IsClientWithSuchNickExist(nick))
        {
            throw new ClientAlreadyExistException(nick);
        }

        NicksAndPasswords.Add(nick, password);
    }

    public bool IsClientsPasswordCorrect(string nick, string password)
    {
        return IsClientWithSuchNickExist(nick) && NicksAndPasswords[nick] == password;
    }
}

public class ClientsDataBaseWithFileStorage : ClientsDataBase
{
    private const string FileName = "ClientsData.txt";

    public void ReadClientsData()
    {
        if (!File.Exists(FileName)) return;
        var readText = File.ReadAllText(FileName);
        NicksAndPasswords = JsonSerializer.Deserialize<Dictionary<string, string>>(readText);
    }

    public void WriteClientsData()
    {
        var threeNumbersString = JsonSerializer.Serialize(NicksAndPasswords) + Environment.NewLine;
        File.WriteAllText(FileName, threeNumbersString);
    }
}