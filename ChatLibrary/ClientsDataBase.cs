using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ChatLibrary;

public class ClientAlreadyExistException : Exception
{
    public ClientAlreadyExistException(string login) : base($"Client with this login already exists: {login}")
    {
    }
}

public class ClientDoesntExistException : Exception
{
    public ClientDoesntExistException(string login) : base($"Client with this login doesn't exists: {login}")
    {
    }
}

public class ClientsDataBase
{
    protected Dictionary<string, string> LoginsAndPasswords = new();

    public bool IsClientWithSuchLoginExist(string login)
    {
        return LoginsAndPasswords.ContainsKey(login);
    }

    public bool TryAddUser(string login, string password)
    {
        if (IsClientWithSuchLoginExist(login))
        {
            return false;
        }

        LoginsAndPasswords.Add(login, password);
        return true;
    }

    public bool IsClientsPasswordCorrect(string login, string password) //TODO: make Try
    {
        return IsClientWithSuchLoginExist(login) && LoginsAndPasswords[login] == password;
    }
}

public class ClientsDataBaseWithFileStorage : ClientsDataBase
{
    private const string FileName = "ClientsData.txt";

    public void ReadClientsData()
    {
        if (!File.Exists(FileName)) return;
        var readText = File.ReadAllText(FileName);
        LoginsAndPasswords = JsonSerializer.Deserialize<Dictionary<string, string>>(readText);
    }

    public void WriteClientsData()
    {
        var threeNumbersString = JsonSerializer.Serialize(LoginsAndPasswords) + Environment.NewLine;
        File.WriteAllText(FileName, threeNumbersString);
    }
}