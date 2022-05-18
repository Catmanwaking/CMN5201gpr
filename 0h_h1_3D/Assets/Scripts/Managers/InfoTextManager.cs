using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class InfoTextManager : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetHintInfoText(int rule)
    {
        text.text = LocalizationSystem.GetLocalizedString($"ID_Hint_{rule}");
    }

    public void SetStartText(int size)
    {
        text.text = $"{size}x{size}x{size}";
    }

    public void SetWinInfoText()
    {
        text.text = Random.Range(0, 5) switch
        {
            0 => ":D",
            1 => "yay!",
            2 => "wow!",
            3 => "gg!",
            _ => "Nice!",
        };
    }
}
