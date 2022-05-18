//Author: Dominik Dohmeier
using UnityEngine;

public static class Extensions
{
    private static readonly Vector3[] directions = {
        Vector3.forward, Vector3.back, Vector3.down,
        Vector3.up, Vector3.right, Vector3.left
    };

    private const float root2Half = 0.707f;

    public static Face DetermineFace(this Vector3 direction)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if (Vector3.Dot(directions[i], direction) > root2Half)
                return (Face)i;
        }
        return Face.Undefined;
    }
}

public enum Face
{
    Front,
    Back,
    Top,
    Bottom,
    Left,
    Right,
    Undefined
}