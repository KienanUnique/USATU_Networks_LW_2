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
    protected Dictionary<string, string> _loginsAndPasswords = new();

    public void SignInNewUser(string login, string password)
    {
        if (_loginsAndPasswords.ContainsKey(login))
        {
            throw new ClientAlreadyExistException(login);
        }

        _loginsAndPasswords.Add(login, password);
    }

    public bool IsClientsPasswordCorrect(string login, string password)
    {
        if (!_loginsAndPasswords.ContainsKey(login))
        {
            throw new ClientDoesntExistException(login);
        }

        return _loginsAndPasswords[login] == password;
    }
}

public class ClientsDataBaseWithFileStorage : ClientsDataBase
{
    private const string FileName = "ClientsData.txt";

    public bool IsFileExists()
    {
        return File.Exists(FileName);
    }

    public void ReadClientsData()
    {
        var readText = File.ReadAllText(FileName);
        _loginsAndPasswords = JsonSerializer.Deserialize<Dictionary<string, string>>(readText);
    }

    public void WriteClientsData(ClientsDataBase clientsDataBase)
    {
        var threeNumbersString = JsonSerializer.Serialize(_loginsAndPasswords) + Environment.NewLine;
        File.WriteAllText(FileName, threeNumbersString);
    }
}