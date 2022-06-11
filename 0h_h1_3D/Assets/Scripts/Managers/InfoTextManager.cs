using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;
using Fast_0h_h1;

[RequireComponent(typeof(TMP_Text))]
public class InfoTextManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    private TMP_Text info;
    private string currentID;
    private bool isDefault;

    private void Awake()
    {
        info = GetComponent<TMP_Text>();      
    }

    private void Start()
    {
        LocalizationSystem.OnLanguageChanged += LanguageChanged;
        currentID = null;
        level.grid.OnTileChanged += OnTileChanged;
        SetDefaultText();
    }

    private void OnTileChanged(Vector3Int _) => SetDefaultText();

    public void SetDefaultText()
    {
        if (isDefault)
            return;

        Settings settings = SettingsLoader.LoadSettings();
        if (settings.UseStopwatch == 1)
        {
            Records records = RecordsLoader.LoadRecords();
            switch (level.grid.SideLength)
            {
                case 4:
                    if (records.recordSize4Exists)
                    {
                        TimeSpan time = new(0,0,(int)records.recordSize4);
                        info.text = time.ToString(@"m\:ss");
                    }
                    else
                        SetSizeText();
                    break;
                case 6:
                    if (records.recordSize6Exists)
                    {
                        TimeSpan time = new(0, 0,(int)records.recordSize6);
                        info.text = time.ToString(@"m\:ss");
                    }
                    else
                        SetSizeText();
                    break;
            }
        }
        else
            SetSizeText();

        currentID = null;
        isDefault = true;
    }

    private void SetSizeText()
    {
        int size = level.grid.SideLength;
        info.text = $"{size}x{size}x{size}";
    }

    public void SetHintInfoText(RuleInfo rule)
    {
        currentID = $"ID_Hint_{(int)rule.brokenRule}";
        info.text = LocalizationSystem.GetLocalizedString(currentID);
        isDefault = false;
    }

    public void SetTutorialText(int index)
    {
        string ID = $"ID_Tutorial_Info_{index}";
        info.text = LocalizationSystem.GetLocalizedString(ID);
        isDefault = true;
    }

    private void LanguageChanged()
    {
        if(!string.IsNullOrEmpty(currentID))
            info.text = LocalizationSystem.GetLocalizedString(currentID);
    }

    public void SetWinInfoText()
    {
        currentID = null;
        info.text = Random.Range(0, 5) switch
        {
            0 => ":D",
            1 => "yay!",
            2 => "wow!",
            3 => "gg!",
            _ => "Nice!",
        };
    }
}
