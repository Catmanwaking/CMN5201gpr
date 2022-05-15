//Author: Dominik Dohmeier
using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    [SerializeField] private Material originalMaterial;
    [SerializeField] private MeshRenderer meshRendererPrefab;

    [SerializeField] private LevelSO level;
    [SerializeField] private GameObject gimbal;
   
    private MeshRenderer[,,] cubes;
    private Material[] materials;

    private void Start()
    {
        LoadColors();
        SetupCubes();
        SetupCamera();
    }

    public void LoadColors()
    {
        if(materials == null)
        {
            materials = new Material[ColorIndex.ColorCount];
            for (int c = 0; c < ColorIndex.ColorCount; c++)
                materials[c] = new Material(originalMaterial);
        }

        for (int c = 0; c < ColorIndex.ColorCount; c++)
            materials[c].SetColor("_BaseColor",ColorIndex.GetColor(c));
    }

    private void SetupCubes()
    {
        if (level.grid == null)
            level.grid = new CubeGrid(2);

        CubeGrid grid = level.grid;
        int sideLength = grid.SideLength;

        cubes = new MeshRenderer[sideLength, sideLength, sideLength];
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    MeshRenderer renderer = Instantiate(meshRendererPrefab);
                    renderer.transform.SetParent(this.transform);
                    renderer.transform.localPosition = new Vector3(x, y, z);

                    int color = grid[x, y, z];
                    renderer.material = materials[color];
                    cubes[x, y, z] = renderer;
                }
            }
        }

        grid.OnTileChanged += OnTileChanged;
    }

    private void SetupCamera()
    {
        float pos = (cubes.GetLength(0) >> 1) - 0.5f;
        gimbal.transform.localPosition = new Vector3(pos, pos, pos);
    }

    private void OnTileChanged(Vector3Int pos)
    {
        int color = level.grid[pos];
        cubes[pos.x, pos.y, pos.z].material = materials[color];
    }
}