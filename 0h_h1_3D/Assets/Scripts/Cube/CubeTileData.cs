//Author: Dominik Dohmeier

using UnityEngine;

public struct CubeTileData
{
    private static Material[][][] materials;
    private static Material originalMaterial;
    private static float outlineThickness;
    private static float alpha;

    private MeshRenderer renderer;
    private BoxCollider collider;
    private int color;
    private int highlighting;
    private int transparent;

    public int Color 
    {
        get => color; 
        set
        {
            color = value;
            renderer.material = materials[color][highlighting][transparent];
        }
    }

    public int Highlighting
    {
        get => highlighting;
        set
        {
            highlighting = value;
            renderer.material = materials[color][highlighting][transparent];
        }
    }

    public int Transparent
    {
        get => transparent;
        set
        {
            transparent = value;
            renderer.material = materials[color][highlighting][transparent];
            collider.enabled = (transparent == 0);
        }
    }

    public CubeTileData(MeshRenderer renderer)
    {
        this.renderer = renderer;
        this.collider = renderer.gameObject.GetComponent<BoxCollider>();
        color = 0;
        highlighting = 0;
        transparent = 0;
    }

    public static void Initialize(Material original, float outlineThickness, float alpha)
    {
        ColorIndex.ColorChanged += UpdateColors;
        CubeTileData.originalMaterial = original;
        CubeTileData.outlineThickness = outlineThickness;
        CubeTileData.alpha = alpha;
        CubeTileData.SetupMaterials();
    }

    public static void UpdateColors()
    {
        for (int color = 0; color < ColorIndex.ColorCount; color++)
        {
            for (int outline = 0; outline < 2; outline++)
            {
                for (int transparent = 0; transparent < 2; transparent++)
                {
                    materials[color][outline][transparent].SetColor("_BaseColor", ColorIndex.GetColor(color));
                    materials[color][outline][transparent].SetColor("_OutlineColor", ColorIndex.GetOutlineColor(outline));
                    materials[color][outline][transparent].SetFloat("_OutlineThickness", outlineThickness);
                    if (transparent == 1)
                        materials[color][outline][transparent].SetFloat("_Alpha", alpha);
                }
            }
        }
    }

    private static void SetupMaterials()
    {
        materials = new Material[ColorIndex.ColorCount][][];
        for (int color = 0; color < ColorIndex.ColorCount; color++)
        {
            materials[color] = new Material[2][];
            for (int outline = 0; outline < 2; outline++)
            {
                materials[color][outline] = new Material[2];
                for (int transparent = 0; transparent < 2; transparent++)
                    materials[color][outline][transparent] = new Material(originalMaterial);
            }
        }
    }
}