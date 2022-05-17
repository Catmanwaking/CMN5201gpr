using UnityEngine;
using TMPro;

public class InfoTextManager : MonoBehaviour
{
    private TMP_Text text;


    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetInfoText(int rule)
    {
        text.text = rule.ToString();
    }
}
