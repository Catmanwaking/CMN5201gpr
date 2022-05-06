//Author: Dominik Dohmeier
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class OptionCycleText : MonoBehaviour
{
    [SerializeField] private bool UsesLocalization;
    [SerializeField] private string[] options;
    public UnityEvent<string> OnOptionChange;
    private TMP_Text text;
    private int index = 0;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        LocalizationSystem.OnLanguageChanged += SetText;
        SetText();
        OnOptionChange.Invoke(options[index]);
    }

    public void CycleNext()
    {
        index = (index + 1) % options.Length;
        SetText();
        OnOptionChange.Invoke(options[index]);
    }

    private void SetText()
    {
        if (!UsesLocalization)
            text.text = options[index];
        else
            text.text = LocalizationSystem.GetLocalizedString(options[index]);        
    }
}