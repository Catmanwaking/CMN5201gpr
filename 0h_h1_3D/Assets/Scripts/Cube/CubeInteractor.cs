using UnityEngine;

public class CubeInteractor : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private Camera cubeCam;
    [SerializeField] private RectTransform playAreaRect;
    [SerializeField] private LayerMask cubeLayer;

    public void OnTouch(Vector2 position)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(playAreaRect, position, null, out Vector2 screenPoint))
        {
            screenPoint += new Vector2(playAreaRect.rect.width, playAreaRect.rect.height) * 0.5f;
            if(RayCastCube(screenPoint, out Vector3Int pos))
                OnTileClicked(pos);
        }
    }

    private bool RayCastCube(Vector2 textureCoord, out Vector3Int position)
    {
        textureCoord *= (cubeCam.pixelWidth / playAreaRect.rect.width); //TODO precalc
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
        Debug.Log(pos);
        int color = level.grid[pos.x, pos.y, pos.z];
        color = (color + 1) % (level.grid.ColorCount + 1);
        level.grid[pos.x, pos.y, pos.z] = color;
    }
}
