using JackStreamBox.Util.Data;
using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

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


    private static string FileNameToCustomFile(string filename)
    {
       return  $"{gitIgnorePart}{filename}.txt";
    }
    public static void WriteCustomData<T>(string filename, T data)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            string filePath = FileNameToCustomFile(filename);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, data);
            }
        }     
        catch
        {
            
        }

    }

    public static T ReadCustomData<T>(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        string filePath = FileNameToCustomFile(filename);
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        catch
        {
            return default(T);
        }

    }


}