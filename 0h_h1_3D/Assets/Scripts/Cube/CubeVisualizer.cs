//Author: Dominik Dohmeier
using Fast_0h_h1;
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
    [SerializeField] private AudioClip rotationSound;

    [Header("MaterialProperties")]
    [SerializeField, Range(0.0f, 1.0f)] private float outlineThickness = 0.1f;
    [SerializeField, Range(0.0f, 1.0f)] private float transparency = 0.4f;

    private LevelSO level;
    private CubeTileData[,,] cubes;

    private bool hasHighlighting;
    private int currentZAxis;
    private int ignoreResetHighlightCount;
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
        CubeTileData.Initialize(originalMaterial, outlineThickness, transparency);
        SetupCubes();
        SetupCamera();
    }

    public void RemoveHighlighting()
    {
        if (!hasHighlighting)
            return;

        if(ignoreResetHighlightCount > 0)
        {
            ignoreResetHighlightCount--;
            return;
        }

        int sideLength = cubes.GetLength(0);
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                    cubes[x, y, z].Highlighting = 0;
            }
        }
        hasHighlighting = false;
    }

    public void SetIgnores(int count)
    {
        ignoreResetHighlightCount = count;
    }

    public void OnSwipeInput(SwipeDirection direction, MonoBehaviour coroutineCaller)
    {
        var rotation = direction switch
        {
            SwipeDirection.Up => Quaternion.AngleAxis(-90.0f, Vector3.right),
            SwipeDirection.Down => Quaternion.AngleAxis(90.0f, Vector3.right),
            SwipeDirection.Left => Quaternion.AngleAxis(-90.0f, Vector3.up),
            SwipeDirection.Right => Quaternion.AngleAxis(90.0f, Vector3.up),
            _ => Quaternion.identity,
        };

        if (rotationCoroutine != null)
            coroutineCaller.StopCoroutine(rotationCoroutine);
        rotationCoroutine = coroutineCaller.StartCoroutine(RotationRoutine(rotation));
    }

    #region Views

    private IEnumerator RotationRoutine(Quaternion rotation)
    {
        AudioManager.PlayAudio(rotationSound, 1.0f, true);

        Quaternion startRotation = gimbal.transform.rotation;
        Quaternion targetRotation = currentEndRotation * rotation;

        gimbal.transform.rotation = currentEndRotation;
        currentEndRotation = targetRotation;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rotationSpeed;
            if (t > 1.0f)
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
        int SideLength = cubes.GetLength(0);
        for (int x = 0; x < SideLength; x++)
        {
            for (int y = 0; y < SideLength; y++)
            {
                for (int z = 0; z < SideLength; z++)
                    cubes[x, y, z].Transparent = 0;
            }
        }
    }

    private void SetFrontView()
    {
        int SideLength = cubes.GetLength(0);
        for (int i = 0; i < currentZAxis; i++)
        {
            for (int x = 0; x < SideLength; x++)
            {
                for (int y = 0; y < SideLength; y++)
                    cubes[x, y, i].Transparent = 1;
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
                    cubes[x, y, SideLength - i - 1].Transparent = 1;
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
                    cubes[x, SideLength - i - 1, z].Transparent = 1;
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
                    cubes[x, i, z].Transparent = 1;
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
                    cubes[i, y, z].Transparent = 1;
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
                    cubes[SideLength - i - 1, y, z].Transparent = 1;
            }
        }
    }

    #endregion Views

    #region Setup

    private void SetupCubes()
    {
        CubeGrid grid = level.grid;
        int sideLength = grid.SideLength;

        cubes = new CubeTileData[sideLength, sideLength, sideLength];
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    MeshRenderer renderer = GameObject.Instantiate(meshRendererPrefab);
                    cubes[x, y, z] = new CubeTileData(renderer);
                    renderer.transform.SetParent(cubeHolder.transform);
                    renderer.transform.localPosition = new Vector3(x, y, z);

                    cubes[x, y, z].Color = grid[x, y, z];
                }
            }
        }

        grid.OnTileChanged += OnTileChanged;
    }

    private void SetupCamera()
    {
        currentEndRotation = gimbal.transform.rotation;
        float pos = (cubes.GetLength(0) >> 1) - 0.5f;
        gimbal.transform.localPosition = new Vector3(pos, pos, pos);
    }

    #endregion Setup

    public void HighlightLine(RuleInfo ruleInfo)
    {
        int direction = ruleInfo.direction;
        Vector3Int pos = new();
        pos[(direction + 1) % 3] = ruleInfo.axis1;
        pos[(direction + 2) % 3] = ruleInfo.axis2;
        for (int i = 0; i < cubes.GetLength(0); i++)
        {
            pos[direction] = i;
            cubes[pos.x, pos.y, pos.z].Highlighting = 1;
        }

        hasHighlighting = true;
    }

    public void HighlightSingle(Vector3Int pos)
    {
        cubes[pos.x, pos.y, pos.z].Highlighting = 1;
        hasHighlighting = true;
    }

    public void ResetCameraAngle(MonoBehaviour coroutineCaller)
    {
        Quaternion rotation = Quaternion.identity;
        currentEndRotation = rotation;
        coroutineCaller.StartCoroutine(RotationRoutine(rotation));              
    }

    private void OnTileChanged(Vector3Int pos)
    {
        cubes[pos.x, pos.y, pos.z].Color = level.grid[pos];
        RemoveHighlighting();
    }
}