//Author: Dominik Dohmeier
using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem
{
    public static event System.Action OnLanguageChanged;

    public static string languageID = "DE";
    private static Dictionary<string, string> localizedDict;

    public static void ChangeLanguage(string languageID)
    {
        LocalizationSystem.languageID = languageID;
        LoadLanguage();
    }

    public static string GetLocalizedString(string ID)
    {
        if (localizedDict == null)
            LoadLanguage();

        if (localizedDict.ContainsKey(ID))
            return localizedDict[ID];

        Debug.LogWarning($"The key \"{ID}\" does not exist in the localization dictionary");

        return string.Empty;
    }

    private static void LoadLanguage()
    {
        CSVLoader loader = new();
        localizedDict = loader.GetDictionaryValues(languageID);
        OnLanguageChanged?.Invoke();
    }
}