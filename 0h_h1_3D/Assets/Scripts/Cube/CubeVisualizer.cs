//Author: Dominik Dohmeier
using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    [SerializeField] private Material originalMaterial;
    [SerializeField] private MeshRenderer meshRendererPrefab;

    [SerializeField] private LevelSO level;
    [SerializeField] private GameObject gimbal;
    [SerializeField] private Camera cam;

    private MeshRenderer[,,] cubes;
    private Material[] materials;

    private void Start()
    {
        LoadColors();

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

        SetupCamera();
    }

    private void SetupCamera()
    {
        float pos = (cubes.GetLength(0) >> 1) - 0.5f;
        float distance = pos + 5.5f; //TODO MAGIC NUMBER
        gimbal.transform.localPosition = new Vector3(pos, pos, pos);

        cam.transform.localPosition = new Vector3(0, 0, -distance);
    }

    public void RotateCube(Vector2 input)
    {
        float rotationStrength = input.magnitude;
        Vector2 perp = new Vector2(input.y, -input.x).normalized;
        transform.Rotate(perp, perp.magnitude, Space.World);
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
            materials[c].color = ColorIndex.GetColor(c);
    }
}