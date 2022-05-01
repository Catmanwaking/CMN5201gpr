//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonScript : MonoBehaviour
{
    private System.Action<Vector2Int> action;

    private Vector2Int gridPos;
    private Button attachedButton;

    public void SetupButton(int posX, int posY, System.Action<Vector2Int> callback)
    {
        gridPos = new Vector2Int(posX, posY);
        action = callback;
        attachedButton = GetComponent<Button>();
        attachedButton.onClick.AddListener(OnClick);
    }

    public void SetColor(int color)
    {
        attachedButton.image.color = ColorIndex.GetColor(color);
    }

    private void OnClick()
    {
        action?.Invoke(gridPos);
    }
}