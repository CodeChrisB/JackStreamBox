using JackStreamBox.Bot.Logic.Commands;
using System;
using System.Collections.Generic;
using System.IO;

public static class BotSetings
{
    private const string FileName = "changeAbleValues.txt";
    private static Dictionary<string, object> dataDictionary;

    public static void LoadBotSetings()
    {
        LoadDataFromFile();
    }

    public static void WriteData<T>(string key, T value)
    {
        dataDictionary[key] = value;
        SaveDataToFile();
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

    private static void SaveDataToFile()
    {
        using (StreamWriter writer = new StreamWriter(FileName))
        {
            foreach (var kvp in dataDictionary)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }
    }
}