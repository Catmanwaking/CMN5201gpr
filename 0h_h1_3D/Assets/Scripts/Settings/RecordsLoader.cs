//Author: Dominik Dohmeier
using System;
using System.IO;
using UnityEngine;

public static class RecordsLoader
{
    private static readonly string path = Application.persistentDataPath;
    private const string fileName = "records.json";

    private static Records records;
    private static bool loaded = false;

    public static Records LoadRecords()
    {
        if (loaded)
            return records;

        if (!File.Exists($"{path}/{fileName}"))
        {
            return new Records
            (
                new TimeSpan(),
                new TimeSpan(),
                0
            );
        }

        string jsonString = File.ReadAllText($"{path}/{fileName}");
        records = JsonUtility.FromJson<Records>(jsonString);
        loaded = true;
        return records;
    }

    public static void SaveRecords(Records records)
    {
        RecordsLoader.records = records;
        string jsonString = JsonUtility.ToJson(records, true);
        File.WriteAllText($"{path}/{fileName}", jsonString);
    }
}