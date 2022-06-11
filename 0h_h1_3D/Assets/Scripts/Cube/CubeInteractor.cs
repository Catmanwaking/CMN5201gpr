using UnityEngine;

[System.Serializable]
public class CubeInteractor
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera cubeCam;
    [SerializeField] private RectTransform playAreaRect;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private AudioClip clickSound;

    private LevelSO level;
    public bool AllowInput = true;

    public void Initialize(LevelSO level)
    {
        this.level = level;
        cubeCam.targetTexture.format = RenderTextureFormat.Default;
        cubeCam.orthographicSize = (level.grid.SideLength >> 1) + 1;
    }

    public void OnTapInput(Vector2 position)
    {
        if (!AllowInput)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(playAreaRect, position, mainCam, out Vector2 screenPoint))
        {
            screenPoint += new Vector2(playAreaRect.rect.width, playAreaRect.rect.height) * 0.5f;
            if(RayCastCube(screenPoint, out Vector3Int pos))
                OnTileClicked(pos);
        }
    }

    private bool RayCastCube(Vector2 textureCoord, out Vector3Int position)
    {
        textureCoord *= (cubeCam.pixelWidth / playAreaRect.rect.width);
        Ray renderTextureRay = cubeCam.ScreenPointToRay(textureCoord);

        if (Physics.Raycast(renderTextureRay, out RaycastHit hitInfo, float.MaxValue, cubeLayer, QueryTriggerInteraction.Ignore))
        {
            position = Vector3Int.FloorToInt(hitInfo.collider.gameObject.transform.position);
            return true;
        }
        position = Vector3Int.zero;
        return false;
    }

    private void OnTileClicked(Vector3Int pos)
    {
        int color = level.grid[pos.x, pos.y, pos.z];
        color = (color + 1) % ColorIndex.ColorCount;
        level.grid[pos.x, pos.y, pos.z] = color;
        AudioManager.PlayAudio(clickSound, 1.0f, true);
    }
}
