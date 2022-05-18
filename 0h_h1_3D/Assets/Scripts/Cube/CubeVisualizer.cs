//Author: Dominik Dohmeier
using System.Collections;
using UnityEngine;

[System.Serializable]
public class CubeVisualizer
{
    [Header("prefabs")]
    [SerializeField] private MeshRenderer meshRendererPrefab;
    [SerializeField] private Material originalMaterial;

    [Header("Transforms")]
    [SerializeField] private GameObject gimbal;
    [SerializeField] private GameObject cubeHolder;

    [Header("Rotation Properties")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AnimationCurve curve;

    private LevelSO level;
    private MeshRenderer[,,] cubes;
    private Material[] materials;

    private int currentZAxis;
    private Coroutine rotationCoroutine;
    private Quaternion currentEndRotation;

    public int CurrentZAxis
    {
        get => currentZAxis;
        set
        {
            if (value < 0 || value >= cubes.GetLength(0))
                return;
            currentZAxis = value;
            SetView();
        }
    }

    public void Initialize(LevelSO level)
    {
        this.level = level;
        ColorIndex.ColorChanged += UpdateColors;
        currentEndRotation = gimbal.transform.rotation;
        SetupMaterials();
        SetupCubes();
        SetupCamera();
    }

    public void UpdateColors()
    {
        for (int c = 0; c < ColorIndex.ColorCount; c++)
            materials[c].SetColor("_BaseColor", ColorIndex.GetColor(c));
    }

    public void OnSwipeInput(SwipeDirection direction, MonoBehaviour coroutineCaller)
    {
        //gimbal.transform.Rotate(Vector3.right, -90.0f, Space.Self);
        var rotation = direction switch
        {
            SwipeDirection.Up => Quaternion.AngleAxis(-90.0f, Vector3.right),
            SwipeDirection.Down => Quaternion.AngleAxis(90.0f, Vector3.right),
            SwipeDirection.Left => Quaternion.AngleAxis(-90.0f, Vector3.up),
            SwipeDirection.Right => Quaternion.AngleAxis(90.0f, Vector3.up),
            _ => Quaternion.identity,
        };       

        if(rotationCoroutine != null)
            coroutineCaller.StopCoroutine(rotationCoroutine);
        rotationCoroutine = coroutineCaller.StartCoroutine(RotationRoutine(rotation));
    }

    #region Views

    private IEnumerator RotationRoutine(Quaternion rotation)
    {
        Quaternion startRotation = gimbal.transform.rotation;
        Quaternion targetRotation = currentEndRotation * rotation;

        gimbal.transform.rotation = currentEndRotation;
        currentEndRotation = targetRotation;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rotationSpeed;
            if(t > 1.0f)
                t = 1.0f;
            gimbal.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curve.Evaluate(t));
            yield return new WaitForEndOfFrame();
        }
        SetView();
    }

    private void SetView()
    {
        ResetView();
        Face facingDir = gimbal.transform.forward.DetermineFace();
        switch (facingDir)
        {
            case Face.Front:
                SetFrontView();
                break;

            case Face.Back:
                SetBackView();
                break;

            case Face.Top:
                SetTopView();
                break;

            case Face.Bottom:
                SetBottomView();
                break;

            case Face.Left:
                SetLeftView();
                break;

            case Face.Right:
                SetRightView();
                break;
        }
    }

    private void ResetView()
    {
        foreach (var item in cubes)
            item.gameObject.SetActive(true);
    }

    private void SetFrontView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                for (int y = 0; y < SideLength; y++)
                    cubes[x, y, i].gameObject.SetActive(false);
            }
        }
    }

    private void SetBackView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                for (int y = 0; y < SideLength; y++)
                    cubes[x, y, SideLength - i - 1].gameObject.SetActive(false);
            }
        }
    }

    private void SetTopView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                for (int z = 0; z < SideLength; z++)
                    cubes[x, SideLength - i - 1, z].gameObject.SetActive(false);
            }
        }
    }

    private void SetBottomView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                for (int z = 0; z < SideLength; z++)
                    cubes[x, i, z].gameObject.SetActive(false);
            }
        }
    }

    private void SetLeftView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int y = 0; y < SideLength; y++)
            {
                for (int z = 0; z < SideLength; z++)
                    cubes[i, y, z].gameObject.SetActive(false);
            }
        }
    }

    private void SetRightView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int y = 0; y < SideLength; y++)
            {
                for (int z = 0; z < SideLength; z++)
                    cubes[SideLength - i - 1, y, z].gameObject.SetActive(false);
            }
        }
    }

    #endregion Views

    #region Setup

    private void SetupMaterials()
    {
        materials = new Material[ColorIndex.ColorCount];
        for (int c = 0; c < ColorIndex.ColorCount; c++)
            materials[c] = new Material(originalMaterial);
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
                    MeshRenderer renderer = GameObject.Instantiate(meshRendererPrefab);
                    renderer.transform.SetParent(cubeHolder.transform);
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

    #endregion Setup

    public void HighlightLine(int ruleInfo)
    {
        int firstAxis = ruleInfo & 0b_0111;
        int secondAxis = (ruleInfo >> 3) & 0b_0111;
        int direction = (ruleInfo >> 6) & 0b_0111;
        Debug.Log($"f:{firstAxis}, s:{secondAxis}, d:{direction}, raw:{ruleInfo}");
    }

    private void OnTileChanged(Vector3Int pos)
    {
        int color = level.grid[pos];
        cubes[pos.x, pos.y, pos.z].material = materials[color];
    }
}