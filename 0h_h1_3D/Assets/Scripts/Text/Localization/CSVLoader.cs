//Author: Dominik Dohmeier
using System.Collections.Generic;
using UnityEngine;

public class CSVLoader
{
    private TextAsset csvFile;
    private char lineSeperator = '\n';
    private char fieldSeperator = ';';
    private char carrigeReturnReplaceChar = '~';
    
    private void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("localization");
    }

    public Dictionary<string, string> GetDictionaryValues(string LanguageID)
    {
        if(csvFile == null)
            LoadCSV();

        Dictionary<string, string> dictionary = new();

        string[] lines = csvFile.text.Split(lineSeperator);
        string[] headers = lines[0].Split(fieldSeperator);
        int languageIndex = -1;
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(LanguageID))
            {
                languageIndex = i;
                break;
            }
        }
        if (languageIndex == -1)
            return null;
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] fields = lines[i].Split(fieldSeperator);

            string key = fields[0];

            if(dictionary.ContainsKey(key))
            {
                Debug.LogWarning($"Duplicate entry in localization file found: {key}");
                continue;
            }

            string value = fields[languageIndex];
            value = value.Replace(carrigeReturnReplaceChar, '\n');
            dictionary.Add(key, value);
        }

        return dictionary;
    }
}