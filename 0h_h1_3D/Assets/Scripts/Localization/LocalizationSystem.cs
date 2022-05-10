//Author: Dominik Dohmeier
using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem
{
    public static event System.Action OnLanguageChanged;

    private static Dictionary<string, string> localizedDict;

    public static string GetLocalizedString(string ID)
    {
        if (localizedDict == null)
            LoadLanguage();

        if (localizedDict.ContainsKey(ID))
            return localizedDict[ID];

        Debug.LogWarning($"The key \"{ID}\" does not exist in the localization dictionary");

        return string.Empty;
    }

    public static void LoadLanguage(string languageID = null)
    {
        if(string.IsNullOrEmpty(languageID))
            languageID = SettingsLoader.LoadLanguage().ToString();

        CSVLoader loader = new();        
        localizedDict = loader.GetDictionaryValues(languageID);
        OnLanguageChanged?.Invoke();
    }
}