using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class InfoTextManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    private TMP_Text info;
    private string currentID;
    bool isDefault;

    private void Awake()
    {
        info = GetComponent<TMP_Text>();      
    }

    private void Start()
    {
        LocalizationSystem.OnLanguageChanged += LanguageChanged;
        currentID = null;
        level.grid.OnTileChanged += OnTileChanged;
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
                        info.text = records.recordSize4.ToString(@"m\:ss");
                    else
                        SetSizeText();
                    break;
                case 6:
                    if (records.recordSize6Exists)
                        info.text = records.recordSize6.ToString(@"m\:ss");
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

    public void SetHintInfoText(int rule)
    {
        currentID = $"ID_Hint_{rule}";
        info.text = LocalizationSystem.GetLocalizedString(currentID);
        isDefault = false;
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
