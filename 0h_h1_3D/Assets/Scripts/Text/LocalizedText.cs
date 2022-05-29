//Author: Dominik Dohmeier
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    private TMP_Text textField;
    [SerializeField] private string ID;

    private void Start()
    {
        textField = GetComponent<TMP_Text>();

        LocalizationChanged();
        LocalizationSystem.OnLanguageChanged += LocalizationChanged;
    }

    private void OnDestroy()
    {
        LocalizationSystem.OnLanguageChanged -= LocalizationChanged;
    }

    private void LocalizationChanged()
    {
        textField.text = LocalizationSystem.GetLocalizedString(ID);
    }
}