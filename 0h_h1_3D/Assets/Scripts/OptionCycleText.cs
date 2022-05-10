//Author: Dominik Dohmeier
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class OptionCycleText : MonoBehaviour
{
    [SerializeField] private bool UsesLocalization;
    [SerializeField] private string[] options;
    public event System.Action OnOptionChange;
    private TMP_Text text;
    private int index = 0;

    public int Index
    {
        get => index;
        set
        {
            index = value;
            SetText();
            OnOptionChange?.Invoke();
        }
    }

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        LocalizationSystem.OnLanguageChanged += SetText;
    }

    private void OnDestroy()
    {
        LocalizationSystem.OnLanguageChanged -= SetText;
    }

    public void CycleNext()
    {
        index = (index + 1) % options.Length;
        SetText();
        OnOptionChange?.Invoke();
    }

    public string GetSelected()
    {
        return options[index];
    }

    private void SetText()
    {
        if (!UsesLocalization)
            text.text = options[index];
        else
            text.text = LocalizationSystem.GetLocalizedString(options[index]);
    }
}