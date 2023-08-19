using JackStreamBox.Util.Data;
using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

public static class BotData
{
    private const string FileName = "config.txt";
    private const string gitIgnorePart = "customConfigData";
    private static Dictionary<string, object> dataDictionary;

    public static void LoadBotSetings()
    {
        LoadDataFromFile();
    }

    //Read/Write to Default Config File
    public static void WriteData<T>(string key, T value)
    {
        dataDictionary[key] = value;

        //Start Up Messages were edited reload them
        if (key.StartsWith("m") && key.Length == 2) BotMessage.ReloadMessages();
        else if (key == "screen") GameOpener.SetWindowPos(); 


        SaveDataToFile(FileName);
    }

    public static int ReadData(string key,int defaultvalue)
    {
        string? value = (string?)dataDictionary.GetValueOrDefault(key);


        return value == null ? defaultvalue :  Int32.Parse(value);
    }

    public static string ReadData(string key, string defaultvalue)
    {
        string? value = (string?)dataDictionary.GetValueOrDefault(key);


        return value == null ? defaultvalue : value;
    }



    private static void LoadDataFromFile()
    {
        dataDictionary = new Dictionary<string, object>();

        if (File.Exists(FileName))
        {
            string[] lines = File.ReadAllLines(FileName);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    var key = parts[0];
                    var value = parts[1].Trim();
                    dataDictionary[key] = value;
                }
            }
        }
    }

    private static void SaveDataToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var kvp in dataDictionary)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }
    }

    public static void AddGameHost()
    {
        int alreadyPlayed = ReadData(BotVals.GAMES_HOSTED, 0);
        alreadyPlayed++;
        WriteData(BotVals.GAMES_HOSTED, alreadyPlayed);
    }



    public static void WriteCustomData<T>(string filename, T data)
    {
        var serializer = new DataContractSerializer(typeof(T));

        string filePath = $"{gitIgnorePart}{filename}.txt"; // Use correct file extension
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        using (var writer = XmlWriter.Create(stream))
        {
            serializer.WriteObject(writer, data);
        }
    }


    public static T ReadCustomData<T>(string filename)
    {
        var serializer = new DataContractSerializer(typeof(T));

        try
        {
            using (var stream = new FileStream($"{gitIgnorePart}{filename}.txt", FileMode.Open))
            using (var reader = XmlReader.Create(stream))
            {
                return (T)serializer.ReadObject(reader);
            }
        }
        catch
        {
            return default(T);
        }

    }


}